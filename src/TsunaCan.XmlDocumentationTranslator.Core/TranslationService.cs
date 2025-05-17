using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TsunaCan.XmlDocumentationTranslator.IntelliSense;
using TsunaCan.XmlDocumentationTranslator.Resources;

namespace TsunaCan.XmlDocumentationTranslator;

/// <summary>
///  Service for translating IntelliSense XML documentation files.
/// </summary>
public class TranslationService
{
    private readonly IIntelliSenseDocumentManager documentManager;
    private readonly ITranslator translator;
    private readonly CoreSettings settings;
    private readonly ILogger<TranslationService> logger;

    /// <summary>
    ///  Initializes a new instance of the <see cref="TranslationService"/> class.
    /// </summary>
    /// <param name="documentManager">IntelliSense XML documentation file manager.</param>
    /// <param name="translator">Translating XML documentation files.</param>
    /// <param name="options">Translating settings.</param>
    /// <param name="logger">Logger.</param>
    public TranslationService(
        IIntelliSenseDocumentManager documentManager,
        ITranslator translator,
        IOptions<CoreSettings> options,
        ILogger<TranslationService> logger)
    {
        this.documentManager = documentManager;
        this.translator = translator;
        this.settings = options.Value;
        this.logger = logger;
        this.logger.LogInformation(Messages.DumpCoreSettings, this.settings.ToString());
    }

    /// <summary>
    ///  Execute the translation process.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task ExecuteAsync()
    {
        var document = this.documentManager.Read(this.settings.SourceDocumentPath);
        var translatedDocument = await this.translator.TranslateAsync(document, this.settings.SourceDocumentLanguage, this.settings.OutputFileCultures);

        var baseFilePath = this.settings.OutputDirectoryPath;
        List<string> outputFiles = [];
        foreach (var result in translatedDocument)
        {
            var outputFilePath = Path.Combine(baseFilePath, result.Key.Name, this.settings.SourceDocumentPath);
            outputFiles.Add(outputFilePath);
            this.documentManager.Write(outputFilePath, result.Value);
        }

        var outputFilePaths = $"[{string.Join(", ", outputFiles)}]";
        this.logger.LogInformation(Messages.Translated, outputFiles.Count, outputFilePaths);
    }
}
