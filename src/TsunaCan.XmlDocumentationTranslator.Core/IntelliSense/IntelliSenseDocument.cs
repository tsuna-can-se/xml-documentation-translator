using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using TsunaCan.XmlDocumentationTranslator.Resources;

namespace TsunaCan.XmlDocumentationTranslator.IntelliSense;

/// <summary>
///  Represents the root element of an IntelliSense XML documentation file.
/// </summary>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[XmlRoot(ElementName = Constants.DocElement, Namespace = null, IsNullable = false)]
public partial class IntelliSenseDocument
{
    /// <summary>
    ///  Gets or sets the assembly information in the documentation.
    /// </summary>
    [XmlElement(Constants.AssemblyElement)]
    public Assembly Assembly { get; set; }

    /// <summary>
    ///  Gets or sets the members element in the documentation.
    /// </summary>
    [XmlAnyElement(Name = Constants.MembersElement)]
    public XmlElement MembersElement { get; set; } = new XmlDocument().CreateElement(Constants.MembersElement);

    /// <summary>
    ///  Sets the members elements in the documentation from an XML string.
    /// </summary>
    /// <param name="xml">XML string.</param>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item>XML string is invalid.</item>
    ///  </list>
    /// </exception>
    public void SetMembersInnerXml(string xml)
    {
        if (string.IsNullOrEmpty(xml))
        {
            this.MembersElement = new XmlDocument().CreateElement(Constants.MembersElement);
            return;
        }

        try
        {
            // Create xml document root.
            XmlDocument internalXmlDoc = new XmlDocument();
            internalXmlDoc.LoadXml($"<{Constants.MembersElement}>{xml}</{Constants.MembersElement}>");

            // Set members element.
            if (internalXmlDoc.DocumentElement == null)
            {
                this.MembersElement = new XmlDocument().CreateElement(Constants.MembersElement);
            }
            else
            {
                this.MembersElement = internalXmlDoc.DocumentElement;
            }
        }
        catch (XmlException ex)
        {
            throw new ArgumentException(Messages.InvalidXmlString, nameof(xml), ex);
        }
    }
}
