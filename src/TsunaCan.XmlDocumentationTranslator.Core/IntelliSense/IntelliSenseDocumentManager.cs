using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using TsunaCan.XmlDocumentationTranslator.Resources;

namespace TsunaCan.XmlDocumentationTranslator.IntelliSense;

/// <summary>
///  Manages the reading and writing of IntelliSense XML documentation files.
/// </summary>
public class IntelliSenseDocumentManager : IIntelliSenseDocumentManager
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
    /// <returns><see cref="IntelliSenseDocumentAccessor"/> object.</returns>
    /// <exception cref="FileNotFoundException">Thrown if <paramref name="intelliSenseDocumentPath"/> file not found.</exception>
    /// <exception cref="InvalidOperationException">
    ///  <list type="bullet">
    ///   <item>Thrown if <paramref name="intelliSenseDocumentPath"/> file is not valid format.</item>
    ///   <item>Thrown if <paramref name="intelliSenseDocumentPath"/> file is empty.</item>
    ///  </list>
    /// </exception>
    public IntelliSenseDocumentAccessor Read(string intelliSenseDocumentPath)
    {
        this.logger.LogDebug(Messages.XmlDocumentLoading, intelliSenseDocumentPath);

        try
        {
            // Load and validate XML format in a single operation.
            var document = XDocument.Load(intelliSenseDocumentPath);

            // Validate that this is a proper IntelliSense XML document format
            ValidateIntelliSenseDocumentFormat(document);

            this.logger.LogInformation(Messages.XmlDocumentLoaded, intelliSenseDocumentPath);
            return new IntelliSenseDocumentAccessor(document);
        }
        catch (XmlException ex)
        {
            // Convert XmlException to InvalidOperationException to maintain API compatibility
            throw new InvalidOperationException(ex.Message, ex);
        }
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
    public void Write(string outputFilePath, IntelliSenseDocument document)
    {
        if (string.IsNullOrEmpty(outputFilePath))
        {
            throw new ArgumentException(Messages.ArgumentIsNullOrEmpty, nameof(outputFilePath));
        }

        this.logger.LogInformation(Messages.XmlDocumentCreating, outputFilePath);

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

    /// <summary>
    ///  Validates that the XML document conforms to the expected IntelliSense XML format.
    /// </summary>
    /// <param name="document">The XML document to validate.</param>
    /// <exception cref="InvalidOperationException">Thrown when the document format is invalid.</exception>
    private static void ValidateIntelliSenseDocumentFormat(XDocument document)
    {
        // Check for root element
        if (document.Root == null)
        {
            throw new InvalidOperationException("XML document is empty or has no root element.");
        }

        // Check that root element is 'doc'
        if (document.Root.Name.LocalName != Constants.DocElement)
        {
            throw new InvalidOperationException($"Root element must be '{Constants.DocElement}', but found '{document.Root.Name.LocalName}'.");
        }

        // Check for required 'assembly' element
        var assemblyElement = document.Root.Element(Constants.AssemblyElement);
        if (assemblyElement == null)
        {
            throw new InvalidOperationException($"Required '{Constants.AssemblyElement}' element is missing.");
        }
    }
}
