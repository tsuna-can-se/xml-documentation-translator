using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    ///  Get the member element count from the XML document.
    /// </summary>
    /// <returns>Member element count.</returns>
    public int GetMemberCount()
        => this.GetMembersElements().Count();

    /// <summary>
    ///  Gets the member elements xml string list from the XML document.
    /// </summary>
    /// <param name="chunkSize">Chunk size.</param>
    /// <returns>Chunked strings.</returns>
    public IEnumerable<string> GetMembers(int chunkSize)
    {
        var current = new StringBuilder();
        foreach (var member in this.GetMembers())
        {
            if (current.Length > 0 && current.Length + member.Length > chunkSize)
            {
                yield return current.ToString();
                current.Clear();
            }

            if (member.Length > chunkSize)
            {
                // memberが大きすぎる場合、単独で返却
                yield return member;
            }
            else
            {
                current.Append(member);
            }
        }

        if (current.Length > 0)
        {
            yield return current.ToString();
        }
    }

    /// <summary>
    ///  Gets the member elements xml string list from the XML document.
    /// </summary>
    /// <returns>Member elements xml string list.</returns>
    internal IEnumerable<string> GetMembers()
        => this.GetMembersElements()
        .Select(m => m.ToString(SaveOptions.DisableFormatting));

    private IEnumerable<XElement> GetMembersElements()
        => this.document.Descendants(Constants.MemberElement);
}
