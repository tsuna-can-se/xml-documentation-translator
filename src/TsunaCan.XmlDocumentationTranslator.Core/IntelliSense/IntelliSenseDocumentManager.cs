using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using TsunaCan.XmlDocumentationTranslator.Resources;

namespace TsunaCan.XmlDocumentationTranslator.IntelliSense;

/// <summary>
///  Manages the reading and writing of IntelliSense XML documentation files.
/// </summary>
public class IntelliSenseDocumentManager
{
    private readonly XmlSerializer serializer;
    private readonly ILogger<IntelliSenseDocumentManager> logger;

    /// <summary>
    ///  Initializes a new instance of the <see cref="IntelliSenseDocumentManager"/> class.
    /// </summary>
    /// <param name="logger">The logger used for logging operations.</param>
    public IntelliSenseDocumentManager(ILogger<IntelliSenseDocumentManager> logger)
    {
        this.serializer = new XmlSerializer(typeof(IntelliSenseDocument));
        this.logger = logger;
    }

    /// <summary>
    ///  Reads an IntelliSense XML documentation file and deserializes it into an <see cref="IntelliSenseDocument"/> object.
    /// </summary>
    /// <param name="intelliSenseDocumentPath">The path to the IntelliSense XML documentation file.</param>
    /// <returns>The deserialized <see cref="IntelliSenseDocument"/> object, or <c>null</c> if the file could not be read.</returns>
    /// <exception cref="FileNotFoundException">Thrown if <paramref name="intelliSenseDocumentPath"/> file not found.</exception>
    /// <exception cref="InvalidOperationException">
    ///  <list type="bullet">
    ///   <item>Thrown if <paramref name="intelliSenseDocumentPath"/> file is not valid format.</item>
    ///   <item>Thrown if <paramref name="intelliSenseDocumentPath"/> file is empty.</item>
    ///  </list>
    /// </exception>
    internal IntelliSenseDocument? Read(string intelliSenseDocumentPath)
    {
        this.logger.LogDebug(Messages.XmlDocumentLoading, intelliSenseDocumentPath);
        using var intelliSenseDocumentStreamReader = new StreamReader(intelliSenseDocumentPath);
        var document = this.serializer.Deserialize(intelliSenseDocumentStreamReader) as IntelliSenseDocument;
        this.logger.LogInformation(Messages.XmlDocumentLoaded, intelliSenseDocumentPath);
        return document;
    }

    /// <summary>
    ///  Writes an <see cref="IntelliSenseDocument"/> object to an XML file.
    /// </summary>
    /// <param name="outputFilePath">The path to the output XML file.</param>
    /// <param name="document">The <see cref="IntelliSenseDocument"/> object to serialize and write.</param>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item>Thrown if <paramref name="outputFilePath"/> is null or empty.</item>
    ///  </list>
    /// </exception>
    internal void Write(string outputFilePath, IntelliSenseDocument document)
    {
        ArgumentException.ThrowIfNullOrEmpty(outputFilePath);

        // Ensure the directory exists
        var directory = Path.GetDirectoryName(outputFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            this.logger.LogInformation(Messages.DirectoryCreated, directory);
        }

        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = new string(' ', 4),
            Encoding = Encoding.UTF8,
        };

        using var writer = XmlWriter.Create(outputFilePath, settings);

        // Specify empty namespaces
        var namespaces = new XmlSerializerNamespaces();
        namespaces.Add(string.Empty, string.Empty);

        this.serializer.Serialize(writer, document, namespaces);
        this.logger.LogInformation(Messages.XmlDocumentCreated, outputFilePath);
    }
}
