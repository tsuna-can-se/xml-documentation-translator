using System.Globalization;
using Microsoft.Extensions.Logging;

namespace TsunaCan.XmlDocumentationTranslator;

/// <summary>
///  Represents the application settings.
/// </summary>
public class Settings
{
    /// <summary>
    ///  Gets or sets the authentication token.
    /// </summary>
    public required string Token { get; set; }

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

    /// <summary>
    ///  Gets or sets the chat endpoint URL.
    /// </summary>
    public required Uri ChatEndPointUrl { get; set; }

    /// <summary>
    ///  Gets or sets the model ID to be used.
    /// </summary>
    public required string ModelId { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        string tokenDisplay = this.Token.Length > 10
            ? $"{new string('*', this.Token.Length - 5)}{this.Token[^5..]}"
            : new string('*', this.Token.Length);
        return $"{nameof(this.Token)}: {tokenDisplay}, " +
            $"{nameof(this.SourceDocumentPath)}: {this.SourceDocumentPath}, " +
            $"{nameof(this.SourceDocumentLanguage)}: {this.SourceDocumentLanguage}, " +
            $"{nameof(this.OutputDirectoryPath)}: {this.OutputDirectoryPath}, " +
            $"{nameof(this.OutputFileLanguages)}: {this.OutputFileLanguagesString()}, " +
            $"{nameof(this.LogLevel)}: {this.LogLevel}, " +
            $"{nameof(this.ChatEndPointUrl)}: {this.ChatEndPointUrl}, " +
            $"{nameof(this.ModelId)}: {this.ModelId}";
    }

    private string OutputFileLanguagesString()
        => $"[{string.Join(',', this.OutputFileLanguages.Select(c => c.EnglishName))}]";
}
