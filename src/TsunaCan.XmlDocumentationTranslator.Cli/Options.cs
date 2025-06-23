using System.Globalization;
using Microsoft.Extensions.Logging;

namespace TsunaCan.XmlDocumentationTranslator.Cli;

/// <summary>
///  Cli options for the XML Documentation Translator.
/// </summary>
internal class Options
{
    /// <summary>
    ///  Gets or sets the path to the source document.
    /// </summary>
    public string SourceDocumentPath { get; set; } = string.Empty;

    /// <summary>
    ///  Gets or sets the path to the output directory.
    /// </summary>
    public string OutputDirectoryPath { get; set; } = Environment.CurrentDirectory;

    /// <summary>
    ///  Gets or sets the language of the source document.
    /// </summary>
    public CultureInfo? SourceDocumentLanguage { get; set; }

    /// <summary>
    ///  Gets or sets the languages for the output files.
    /// </summary>
    public CultureInfo[] OutputFileLanguages { get; set; } = [];

    /// <summary>
    ///  Gets or sets the authentication token.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    ///  Gets or sets the chat endpoint URL.
    ///  Default is "https://models.inference.ai.azure.com".
    /// </summary>
    public Uri? ChatEndPointUrl { get; set; }

    /// <summary>
    ///  Gets or sets the model ID to be used.
    /// </summary>
    public string? ModelId { get; set; }

    /// <summary>
    ///  Gets or sets the chunk size for the translation process.
    /// </summary>
    public int? ChunkSize { get; set; }

    /// <summary>
    ///  Gets or sets the maximum number of concurrent requests for AI translation.
    /// </summary>
    public int? MaxConcurrentRequests { get; set; }

    /// <summary>
    ///  Gets or sets a value indicating whether help is requested.
    /// </summary>
    public bool IsHelpRequested { get; set; } = false;

    /// <summary>
    ///  Gets or sets the log level.
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    ///  Validate the options.
    /// </summary>
    /// <param name="errorMessages">Error messages.</param>
    /// <returns>Valid when return <see langword="true"/>.</returns>
    internal bool IsValid(out List<string> errorMessages)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(this.SourceDocumentPath))
        {
            errors.Add("--source-document-path or -s is not set.");
        }

        if (this.OutputFileLanguages.Length == 0)
        {
            errors.Add("--output-file-languages or -l is not set.");
        }

        if (string.IsNullOrEmpty(this.Token))
        {
            errors.Add("--token or -t are not set.");
        }

        errorMessages = errors;
        return errors.Count == 0;
    }
}
