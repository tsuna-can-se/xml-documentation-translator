using System.ComponentModel;
using System.Xml.Serialization;

namespace TsunaCan.XmlDocumentationTranslator.IntelliSense;

/// <summary>
///  Represents the root element of an IntelliSense XML documentation file.
/// </summary>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[XmlRoot(ElementName = "doc", Namespace = null, IsNullable = false)]
public partial class IntelliSenseDocument
{
    /// <summary>
    ///  Gets or sets the assembly information in the documentation.
    /// </summary>
    [XmlElement("assembly")]
    public required Assembly Assembly { get; set; }

    /// <summary>
    ///  Gets or sets the collection of members described in the documentation.
    /// </summary>
    [XmlArrayItem("member", IsNullable = false)]
    [XmlArray("members")]
    public required Member[] Members { get; set; }
}
