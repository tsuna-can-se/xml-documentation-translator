using System.Xml.Linq;

namespace TsunaCan.XmlDocumentationTranslator.IntelliSense;

public class IntelliSenseDocumentAccessorTest
{
    [Fact]
    public void GetAssemblyName_ShouldReturnCorrectAssemblyName()
    {
        // Arrange
        var xml = """
            <doc>
                <assembly>
                    <name>TestAssembly</name>
                </assembly>
            </doc>
            """;
        var document = XDocument.Parse(xml);
        var accessor = new IntelliSenseDocumentAccessor(document);

        // Act
        var result = accessor.GetAssemblyName();

        // Assert
        Assert.Equal("TestAssembly", result);
    }

    [Fact]
    public void GetAssemblyName_ShouldReturnEmptyString_WhenAssemblyElementIsMissing()
    {
        // Arrange
        var xml = @"<doc></doc>";
        var document = XDocument.Parse(xml);
        var accessor = new IntelliSenseDocumentAccessor(document);

        // Act
        var result = accessor.GetAssemblyName();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetMembers_ShouldReturnAllMemberElementsAsXmlStrings()
    {
        // Arrange
        var xml = """
            <doc>
                <members>
                    <member name="M:TestClass.Method1">Method1 Summary</member>
                    <member name="M:TestClass.Method2">Method2 Summary</member>
                </members>
            </doc>
            """;
        var document = XDocument.Parse(xml);
        var accessor = new IntelliSenseDocumentAccessor(document);

        // Act
        var result = accessor.GetMembers().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains("<member name=\"M:TestClass.Method1\">Method1 Summary</member>", result);
        Assert.Contains("<member name=\"M:TestClass.Method2\">Method2 Summary</member>", result);
    }

    [Fact]
    public void GetMembers_ShouldReturnEmptyList_WhenNoMemberElementsExist()
    {
        // Arrange
        var xml = @"<doc></doc>";
        var document = XDocument.Parse(xml);
        var accessor = new IntelliSenseDocumentAccessor(document);

        // Act
        var result = accessor.GetMembers();

        // Assert
        Assert.Empty(result);
    }
}
