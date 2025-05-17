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
    ///  example: "en,ja,zh".
    /// </summary>
    public required string OutputFileLanguages { get; set; }

    /// <summary>
    ///  Gets or sets the language of the output files.
    /// </summary>
    public CultureInfo[] OutputFileCultures =>
        this.OutputFileLanguages.Split(',')
            .Select(c => new CultureInfo(c.Trim()))
            .ToArray();

    /// <summary>
    ///  Gets or sets the log level.
    ///  Default is <see cref="LogLevel.Information"/>.
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{nameof(this.SourceDocumentPath)}: {this.SourceDocumentPath}, " +
            $"{nameof(this.SourceDocumentLanguage)}: {this.SourceDocumentLanguage}, " +
            $"{nameof(this.OutputDirectoryPath)}: {this.OutputDirectoryPath}, " +
            $"{nameof(this.OutputFileLanguages)}: {this.OutputFileLanguages}, " +
            $"{nameof(this.LogLevel)}: {this.LogLevel}";
    }
}
