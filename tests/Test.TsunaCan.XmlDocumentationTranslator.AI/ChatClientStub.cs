using System.Text.RegularExpressions;
using Microsoft.Extensions.AI;

namespace Test.TsunaCan.XmlDocumentationTranslator.AI;

/// <summary>
///  Stub class for <see cref="IChatClient"/>.
/// </summary>
internal partial class ChatClientStub : IChatClient
{
    private static readonly Regex TargetLanguage = TargetLanguageRegex();

    public void Dispose()
    {
    }

    public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
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
        return Task.FromResult(new ChatResponse(chatMessage));
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
}
