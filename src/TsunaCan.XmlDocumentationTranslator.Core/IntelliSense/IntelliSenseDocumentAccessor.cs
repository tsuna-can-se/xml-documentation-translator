using System.Xml.Linq;

namespace TsunaCan.XmlDocumentationTranslator.IntelliSense;

/// <summary>
///  IntelliSense XML documentation file accessor.
/// </summary>
public class IntelliSenseDocumentAccessor
{
    private readonly XDocument document;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntelliSenseDocumentAccessor"/> class.
    /// </summary>
    /// <param name="document"><see cref="XDocument"/> object.</param>
    public IntelliSenseDocumentAccessor(XDocument document)
        => this.document = document;

    /// <summary>
    /// Gets the assembly name element value from the XML document.
    /// </summary>
    /// <returns>Assembly name.</returns>
    public string GetAssemblyName()
        => this.document.Root?.Element(Constants.AssemblyElement)?.Element(Constants.NameElement)?.Value ?? string.Empty;

    /// <summary>
    /// Gets the member elements xml string list from the XML document.
    /// </summary>
    /// <returns>Member elements xml string list.</returns>
    public IEnumerable<string> GetMembers()
        => this.document.Descendants(Constants.MemberElement)
            .Select(m => m.ToString(SaveOptions.DisableFormatting)); // get member element xml string.
}
