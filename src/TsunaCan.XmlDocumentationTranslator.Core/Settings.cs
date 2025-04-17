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
    public required string IntelliSenseDocumentPath { get; set; }

    /// <summary>
    ///  Gets or sets the locale of the IntelliSense document.
    /// </summary>
    public required CultureInfo IntelliSenseDocumentLocale { get; set; }

    /// <summary>
    ///  Gets or sets the output file path.
    /// </summary>
    public required string OutputFilePath { get; set; }

    /// <summary>
    ///  Gets or sets the locale of the output file.
    /// </summary>
    public required CultureInfo OutputFileLocale { get; set; }

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
            $"{nameof(this.IntelliSenseDocumentPath)}: {this.IntelliSenseDocumentPath}, " +
            $"{nameof(this.IntelliSenseDocumentLocale)}: {this.IntelliSenseDocumentLocale}, " +
            $"{nameof(this.OutputFilePath)}: {this.OutputFilePath}, " +
            $"{nameof(this.OutputFileLocale)}: {this.OutputFileLocale}, " +
            $"{nameof(this.LogLevel)}: {this.LogLevel}, " +
            $"{nameof(this.ChatEndPointUrl)}: {this.ChatEndPointUrl}, " +
            $"{nameof(this.ModelId)}: {this.ModelId}";
    }
}
