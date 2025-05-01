using System.Text.RegularExpressions;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;

namespace Test.TsunaCan.XmlDocumentationTranslator.AI;

/// <summary>
///  Stub class for <see cref="IChatClient"/>.
/// </summary>
internal partial class ChatClientStub : IChatClient
{
    private static readonly Regex TargetLanguage = TargetLanguageRegex();
    private readonly int delayMilliseconds = 0;
    private readonly ILogger logger;
    private int parallelCount = 0;

    /// <summary>
    ///  Initializes a new instance of the <see cref="ChatClientStub"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="delayMilliseconds">
    ///  Delay milliseconds in <see cref="GetResponseAsync(IEnumerable{ChatMessage}, ChatOptions?, CancellationToken)"/> method.
    ///  Default is 0.
    /// </param>
    internal ChatClientStub(ILogger<ChatClientStub> logger, int delayMilliseconds = 0)
    {
        this.logger = logger;
        this.delayMilliseconds = delayMilliseconds;
    }

    public int MaxParallelCount { get; set; } = 0;

    public void Dispose()
    {
    }

    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        try
        {
            this.IncrementParallelCount();
            await Task.Delay(this.delayMilliseconds, CancellationToken.None);
            var message = messages.Last();
            var prompt = message.Contents.First();
            var promptStr = prompt.ToString() ?? string.Empty;

            // Get the target language from the prompt string
            if (!TargetLanguage.IsMatch(promptStr))
            {
                throw new InvalidOperationException("Target language not found in the prompt.");
            }

            var targetLanguage = TargetLanguage.Match(promptStr).Groups["lang"].Value;
            var xml = message.Contents.Last().ToString() ?? string.Empty;

            // Set the target language in the XML response.
            xml += targetLanguage;
            var responseContent = $"""
            ```xml
            {xml}
            ```
            """;
            var chatMessage = new ChatMessage(
                ChatRole.Assistant,
                [new TextContent(responseContent)]);
            return new ChatResponse(chatMessage);
        }
        finally
        {
            this.DecrementParallelCount();
        }
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [GeneratedRegex(@"Please translate this XML document into (?<lang>[^\.]+)\.", RegexOptions.Singleline)]
    private static partial Regex TargetLanguageRegex();

    private void IncrementParallelCount()
    {
        lock (this)
        {
            this.parallelCount++;
            this.MaxParallelCount = Math.Max(this.parallelCount, this.MaxParallelCount);
            this.logger.LogInformation("[Increment {time}] Current parallel count: {Count}", DateTime.Now.ToString("HH:mm.ss fffffff"), this.parallelCount);
        }
    }

    private void DecrementParallelCount()
    {
        lock (this)
        {
            this.parallelCount--;
            this.logger.LogInformation("[Decrement {time}] Current parallel count: {Count}", DateTime.Now.ToString("HH:mm.ss fffffff"), this.parallelCount);
        }
    }
}
