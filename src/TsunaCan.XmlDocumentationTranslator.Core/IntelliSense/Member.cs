using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using TsunaCan.XmlDocumentationTranslator.Resources;

namespace TsunaCan.XmlDocumentationTranslator.IntelliSense;

/// <summary>
///  Represents a member in the IntelliSense XML documentation.
/// </summary>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
public partial class Member
{
    /// <summary>
    ///  Gets or sets the name attribute value of the member.
    /// </summary>
    [XmlAttribute("name")]
    public required string Name { get; set; }

    /// <summary>
    ///  Gets or sets the XML elements contained within the member.
    /// </summary>
    [XmlAnyElement]
    public XmlElement[]? InnerElements { get; set; }

    /// <summary>
    ///  Gets or sets the concatenated XML content of the <see cref="InnerElements"/>.
    /// </summary>
    [XmlIgnore]
    public string InnerXml
    {
        get
        {
            if (this.InnerElements == null || this.InnerElements.Length == 0)
            {
                return string.Empty;
            }

            var sb = new System.Text.StringBuilder();
            foreach (var element in this.InnerElements)
            {
                sb.Append(element.OuterXml);
            }

            return sb.ToString();
        }

        set
        {
            if (value is null)
            {
                this.InnerElements = null;
                return;
            }
            else if (string.IsNullOrEmpty(value))
            {
                this.InnerElements = [];
                return;
            }

            try
            {
                // Creates a root element for the XML document.
                XmlDocument internalXmlDoc = new();
                internalXmlDoc.LoadXml($"<root>{value}</root>");

                // Retrieves all child elements of the root element.
                var elements = new List<XmlElement>();
                if (internalXmlDoc.DocumentElement == null)
                {
                    this.InnerElements = null;
                }
                else
                {
                    foreach (XmlNode node in internalXmlDoc.DocumentElement.ChildNodes)
                    {
                        if (node is XmlElement element)
                        {
                            elements.Add(element);
                        }
                    }

                    this.InnerElements = [.. elements];
                }
            }
            catch (XmlException ex)
            {
                throw new ArgumentException(Messages.UnableToConvertObjectToXml, nameof(value), ex);
            }
        }
    }
}
