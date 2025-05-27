using System.ComponentModel;
using System.Xml.Serialization;

namespace TsunaCan.XmlDocumentationTranslator.IntelliSense
{
/// <summary>
///  Represents an assembly in the XML documentation.
/// </summary>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
public partial class Assembly
{
    /// <summary>
    ///  Gets or sets the name element value of the assembly.
    /// </summary>
    [XmlElement(Constants.NameElement)]
    public required string Name { get; set; }
    }
}
