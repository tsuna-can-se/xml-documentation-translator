using System.Globalization;
using TsunaCan.XmlDocumentationTranslator.Resources;

namespace TsunaCan.XmlDocumentationTranslator;

/// <summary>
///  Represents the application settings.
/// </summary>
public class CoreSettings
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
            .Select(c => ConvertTo(c.Trim()))
            .ToArray();

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{nameof(this.SourceDocumentPath)}: {this.SourceDocumentPath}, " +
            $"{nameof(this.SourceDocumentLanguage)}: {this.SourceDocumentLanguage}, " +
            $"{nameof(this.OutputDirectoryPath)}: {this.OutputDirectoryPath}, " +
            $"{nameof(this.OutputFileLanguages)}: {this.OutputFileLanguages}";
    }

    private static CultureInfo ConvertTo(string name)
    {
        try
        {
            return new CultureInfo(name);
        }
        catch (Exception ex)
        {
            throw new ArgumentException(Messages.InvalidCultureName, nameof(name), ex);
        }
    }
}
