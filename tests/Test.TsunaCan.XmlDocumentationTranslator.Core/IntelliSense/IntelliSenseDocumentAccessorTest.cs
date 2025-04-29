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

    [Fact]
    public void GetMembers_ChunkSizeIsSmallerThanMemberElement()
    {
        // Arrange
        var xml = """
            <doc>
                <members>
                    <member>123</member>
                    <member>456</member>
                </members>
            </doc>
            """;
        var document = XDocument.Parse(xml);
        var accessor = new IntelliSenseDocumentAccessor(document);

        // Act
        var result = accessor.GetMembers(20);

        // Assert
        Assert.Collection(
            result,
            item => Assert.Equal("<member>123</member>", item),
            item => Assert.Equal("<member>456</member>", item));
    }

    [Fact]
    public void GetMembers_ChunkSizeComplexPattern()
    {
        // Arrange
        var xml = """
            <doc>
                <members>
                    <member>1</member>
                    <member>2</member>
                    <member>12312345678901234567890</member>
                    <member>3</member>
                    <member>45612345678901234567890</member>
                    <member>4</member>
                </members>
            </doc>
            """;
        var document = XDocument.Parse(xml);
        var accessor = new IntelliSenseDocumentAccessor(document);

        // Act
        var result = accessor.GetMembers(40);

        // Assert
        Assert.Collection(
            result,
            item => Assert.Equal("<member>1</member><member>2</member>", item),
            item => Assert.Equal("<member>12312345678901234567890</member>", item),
            item => Assert.Equal("<member>3</member>", item),
            item => Assert.Equal("<member>45612345678901234567890</member>", item),
            item => Assert.Equal("<member>4</member>", item));
    }
}
