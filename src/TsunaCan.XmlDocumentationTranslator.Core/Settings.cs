using System.Globalization;
using Microsoft.Extensions.Logging;

namespace TsunaCan.XmlDocumentationTranslator;

/// <summary>
///  Represents the application settings.
/// </summary>
public class Settings
{
    /// <summary>
    ///  Gets or sets the path to the IntelliSense document.
    /// </summary>
    public required string SourceDocumentPath { get; set; }

    /// <summary>
    ///  Gets or sets the language of the IntelliSense document.
    /// </summary>
    public required CultureInfo SourceDocumentLanguage { get; set; }

    /// <summary>
    ///  Gets or sets the output directory path.
    /// </summary>
    public required string OutputDirectoryPath { get; set; }

    /// <summary>
    ///  Gets or sets the language of the output files.
    /// </summary>
    public required CultureInfo[] OutputFileLanguages { get; set; }

    /// <summary>
    ///  Gets or sets the log level.
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{nameof(this.SourceDocumentPath)}: {this.SourceDocumentPath}, " +
            $"{nameof(this.SourceDocumentLanguage)}: {this.SourceDocumentLanguage}, " +
            $"{nameof(this.OutputDirectoryPath)}: {this.OutputDirectoryPath}, " +
            $"{nameof(this.OutputFileLanguages)}: {this.OutputFileLanguagesString()}, " +
            $"{nameof(this.LogLevel)}: {this.LogLevel}";
    }

    private string OutputFileLanguagesString()
        => $"[{string.Join(',', this.OutputFileLanguages.Select(c => c.EnglishName))}]";
}
