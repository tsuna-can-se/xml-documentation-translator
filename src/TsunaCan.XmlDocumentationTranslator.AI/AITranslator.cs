using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using TsunaCan.XmlDocumentationTranslator.AI.Resources;
using TsunaCan.XmlDocumentationTranslator.IntelliSense;

namespace TsunaCan.XmlDocumentationTranslator.AI;

/// <summary>
///  <see cref="ITranslator"/> implementation for translating XML documentation using <see cref="IChatClient"/>.
/// </summary>
public partial class AITranslator : ITranslator, IDisposable
{
    private static readonly Regex XmlCodeBlock = XmlCodeBlockRegex();
    private readonly SemaphoreSlim aiSemaphore;
    private readonly Settings settings;
    private readonly ILogger<AITranslator> logger;
    private IChatClient chatClient;
    private bool disposedValue;

    /// <summary>
    ///  Initialize a new instance of the <see cref="AITranslator"/> class.
    /// </summary>
    /// <param name="settings">Application settings.</param>
    /// <param name="chatClient">Chat client of <see cref="Microsoft.Extensions.AI"/>.</param>
    /// <param name="logger">Logger.</param>
    public AITranslator(
        Settings settings,
        IChatClient chatClient,
        ILogger<AITranslator> logger)
    {
        this.settings = settings;
        this.chatClient = chatClient;
        this.logger = logger;
        this.aiSemaphore = new SemaphoreSlim(this.settings.MaxConcurrentRequests);
    }

    /// <inheritdoc />
    public async Task<Dictionary<CultureInfo, IntelliSenseDocument>> TranslateAsync(
        IntelliSenseDocumentAccessor document,
        CultureInfo sourceLanguage,
        IEnumerable<CultureInfo> targetLanguages)
    {
        if (targetLanguages == null || !targetLanguages.Any())
        {
            throw new ArgumentException(Messages.TargetLanguagesCannotBeEmpty, nameof(targetLanguages));
        }

        this.LogDocumentDetails(document);
        var returnValue = InitializeReturnValue(document, targetLanguages);
        var translationTasks = this.StartTranslationTasks(document, sourceLanguage, targetLanguages);
        var translatedXmlDic = await WaitTranslationTasksAsync(translationTasks);
        foreach (var translatedXmlItem in translatedXmlDic)
        {
            returnValue[translatedXmlItem.Key].SetMembersInnerXml(translatedXmlItem.Value.ToString());
        }

        return returnValue;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///  Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">
    ///  This parameter should be <see langword="false"/> when called from a finalizer,
    ///  and <see langword="true"/> when called from the <see cref="Dispose()"/> method.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.chatClient.Dispose();
            }

            this.chatClient = null!;
            this.disposedValue = true;
        }
    }

    private static Dictionary<CultureInfo, IntelliSenseDocument> InitializeReturnValue(
        IntelliSenseDocumentAccessor document,
        IEnumerable<CultureInfo> targetLanguages)
    {
        var assemblyName = document.GetAssemblyName();
        var returnValue = new Dictionary<CultureInfo, IntelliSenseDocument>();

        foreach (var target in targetLanguages)
        {
            var returnDocument = new IntelliSenseDocument
            {
                Assembly = new Assembly
                {
                    Name = assemblyName,
                },
            };
            returnValue.Add(target, returnDocument);
        }

        return returnValue;
    }

    private static async Task<Dictionary<CultureInfo, StringBuilder>> WaitTranslationTasksAsync(
        List<Task<TranslateResult>> translationTasks)
    {
        var translatedXmlDic = new Dictionary<CultureInfo, StringBuilder>();

        await foreach (var task in Task.WhenEach(translationTasks))
        {
            var result = await task;
            if (!translatedXmlDic.TryGetValue(result.TargetLanguage, out var value))
            {
                translatedXmlDic.Add(result.TargetLanguage, new StringBuilder(result.TranslatedXml));
            }
            else
            {
                value.Append(result.TranslatedXml);
            }
        }

        return translatedXmlDic;
    }

    [GeneratedRegex(@"```(?:xml)\s*(.*?)\s*```", RegexOptions.Singleline)]
    private static partial Regex XmlCodeBlockRegex();

    private void LogDocumentDetails(IntelliSenseDocumentAccessor document)
    {
        var assemblyName = document.GetAssemblyName();
        this.logger.LogInformation(Messages.DumpAssemblyName, assemblyName);
        this.logger.LogInformation(Messages.DumpMemberCount, document.GetMemberCount());
    }

    private List<Task<TranslateResult>> StartTranslationTasks(
        IntelliSenseDocumentAccessor document,
        CultureInfo sourceLanguage,
        IEnumerable<CultureInfo> targetLanguages)
    {
        var tasks = new List<Task<TranslateResult>>();

        foreach (var chunkedXml in document.GetMembers(this.settings.ChunkSize))
        {
            foreach (var targetLanguage in targetLanguages)
            {
                tasks.Add(this.TranslateXmlAsync(chunkedXml, sourceLanguage, targetLanguage));
            }
        }

        return tasks;
    }

    private Task<TranslateResult> TranslateXmlAsync(string xml, CultureInfo sourceLanguage, CultureInfo targetLanguage)
    {
        return Task.Run<TranslateResult>(async () =>
        {
            var options = new ChatOptions() { Temperature = 0.01f };
            var prompt = new TextContent($"""
            You are a professional .NET library developer.
            The following XML document is part of the IntelliSense XML documentation that is included in the NuGet package.
            It represents class and method descriptions, parameters, return values, and exception descriptions.
            This XML document is written in {sourceLanguage.EnglishName}.
            Please translate this XML document into {targetLanguage.EnglishName}.
            Please return only the translated XML document.
            """);
            var chatMessage = new ChatMessage(
                ChatRole.Assistant,
                [prompt, new TextContent(xml)]);
            try
            {
                await this.aiSemaphore.WaitAsync(); // Use instance field
                var response = await this.chatClient.GetResponseAsync(chatMessage, options);
                if (XmlCodeBlock.IsMatch(response.Text))
                {
                    return new(
                        targetLanguage,
                        XmlCodeBlock.Match(response.Text).Groups[1].Value);
                }
                else
                {
                    this.logger.LogWarning(Messages.UnexpectedReturnValue, response.Text);
                    return new(targetLanguage, response.Text);
                }
            }
            finally
            {
                this.aiSemaphore.Release(); // Use instance field
            }
        });
    }

    private record TranslateResult(CultureInfo TargetLanguage, string TranslatedXml);
}
