namespace TsunaCan.XmlDocumentationTranslator.IntelliSense;

/// <summary>
///  Interface of manages the reading and writing of IntelliSense XML documentation files.
/// </summary>
public interface IIntelliSenseDocumentManager
{
    /// <summary>
    ///  Reads an IntelliSense XML documentation file and deserializes it into an <see cref="IntelliSenseDocument"/> object.
    /// </summary>
    /// <param name="intelliSenseDocumentPath">The path to the IntelliSense XML documentation file.</param>
    /// <returns><see cref="IntelliSenseDocumentAccessor"/> object.</returns>
    IntelliSenseDocumentAccessor Read(string intelliSenseDocumentPath);

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
    void Write(string outputFilePath, IntelliSenseDocument document);
}
