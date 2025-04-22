using TsunaCan.XmlDocumentationTranslator.Resources;

namespace TsunaCan.XmlDocumentationTranslator.IntelliSense;

public class IntelliSenseDocumentTest
{
    [Fact]
    public void SetMembersInnerXml_ValidXml_SetsMembersElement()
    {
        // Arrange
        var document = new IntelliSenseDocument()
        {
            Assembly = new()
            {
                Name = "TestAssembly",
            },
        };
        var validXml = "<member name=\"T:Example\"></member>";

        // Act
        document.SetMembersInnerXml(validXml);

        // Assert
        Assert.NotNull(document.MembersElement);
        Assert.Equal("members", document.MembersElement.Name);
        Assert.Contains("T:Example", document.MembersElement.InnerXml);
    }

    [Fact]
    public void SetMembersInnerXml_NullOrEmptyXml_ResetsMembersElement()
    {
        // Arrange
        var document = new IntelliSenseDocument()
        {
            Assembly = new()
            {
                Name = "TestAssembly",
            },
        };

        // Act
        document.SetMembersInnerXml(null);

        // Assert
        Assert.NotNull(document.MembersElement);
        Assert.Equal("members", document.MembersElement.Name);
        Assert.Empty(document.MembersElement.InnerXml);
    }

    [Fact]
    public void SetMembersInnerXml_InvalidXml_ThrowsArgumentException()
    {
        // Arrange
        var document = new IntelliSenseDocument()
        {
            Assembly = new()
            {
                Name = "TestAssembly",
            },
        };
        var invalidXml = "<member name=\"T:Example\">";

        // Act
        var action = () => document.SetMembersInnerXml(invalidXml);

        // Assert
        var exception = Assert.Throws<ArgumentException>("xml", action);
        Assert.StartsWith(Messages.InvalidXmlString, exception.Message);
    }
}
