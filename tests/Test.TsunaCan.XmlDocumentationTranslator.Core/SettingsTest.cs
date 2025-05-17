using System.Globalization;
using Microsoft.Extensions.Logging;

namespace TsunaCan.XmlDocumentationTranslator;

public class SettingsTest
{
    [Fact]
    public void ToString_MultipleOutputFileLanguages()
    {
        // Arrange
        var settings = new Settings
        {
            SourceDocumentPath = "path/to/document",
            SourceDocumentLanguage = new CultureInfo("en-US"),
            OutputDirectoryPath = "path/to/output",
            OutputFileLanguages = "ja,es",
        };

        // Act
        var result = settings.ToString();

        // Assert
        var expected = "SourceDocumentPath: path/to/document, " +
                       "SourceDocumentLanguage: en-US, " +
                       "OutputDirectoryPath: path/to/output, " +
                       "OutputFileLanguages: ja,es";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var settings = new Settings
        {
            SourceDocumentPath = "test-path",
            SourceDocumentLanguage = new CultureInfo("fr-FR"),
            OutputDirectoryPath = "output-path",
            OutputFileLanguages = "de-DE",
        };

        // Assert
        Assert.Equal("test-path", settings.SourceDocumentPath);
        Assert.Equal(new CultureInfo("fr-FR"), settings.SourceDocumentLanguage);
        Assert.Equal("output-path", settings.OutputDirectoryPath);
        Assert.Equal("de-DE", settings.OutputFileLanguages);
        Assert.Single(settings.OutputFileCultures, new CultureInfo("de-DE"));
    }

    [Fact]
    public void OutputFileCultures_InvalidCultureName()
    {
        // Arrange
        var settings = new Settings
        {
            SourceDocumentPath = "path/to/document",
            SourceDocumentLanguage = new CultureInfo("en-US"),
            OutputDirectoryPath = "path/to/output",
            OutputFileLanguages = "dummy",
        };

        // Act
        var action = () => settings.OutputFileCultures;

        // Assert
        Assert.Throws<ArgumentException>("name", action);
    }
}
