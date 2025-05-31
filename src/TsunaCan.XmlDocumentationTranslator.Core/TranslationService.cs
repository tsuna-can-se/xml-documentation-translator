using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<TranslationService> logger;

    /// <summary>
    ///  Initializes a new instance of the <see cref="TranslationService"/> class.
    /// </summary>
    /// <param name="documentManager">IntelliSense XML documentation file manager.</param>
    /// <param name="translator">Translating XML documentation files.</param>
    /// <param name="logger">Logger.</param>
    public TranslationService(
        IIntelliSenseDocumentManager documentManager,
        ITranslator translator,
        ILogger<TranslationService> logger)
    {
        this.documentManager = documentManager;
        this.translator = translator;
        this.logger = logger;
    }

    /// <summary>
    ///  Execute the translation process.
    /// </summary>
    /// <param name="sourceDocumentPath">Source document path.</param>
    /// <param name="sourceDocumentLanguage">Source document language.</param>
    /// <param name="outputDirectoryPath">Output directory path.</param>
    /// <param name="outputFileCultures">Output file cultures.</param>
    /// <returns>Task.</returns>
    public async Task ExecuteAsync(string sourceDocumentPath, CultureInfo sourceDocumentLanguage, string outputDirectoryPath, IEnumerable<CultureInfo> outputFileCultures)
    {
        var document = this.documentManager.Read(sourceDocumentPath);
        var translatedDocument = await this.translator.TranslateAsync(document, sourceDocumentLanguage, outputFileCultures);

        List<string> outputFiles = [];
        foreach (var result in translatedDocument)
        {
            var outputFilePath = Path.Combine(outputDirectoryPath, result.Key.Name, sourceDocumentPath);
            outputFiles.Add(outputFilePath);
            this.documentManager.Write(outputFilePath, result.Value);
        }

        var outputFilePaths = $"[{string.Join(", ", outputFiles)}]";
        this.logger.LogInformation(Messages.Translated, outputFiles.Count, outputFilePaths);
    }
}
