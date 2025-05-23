﻿using System.Globalization;

namespace TsunaCan.XmlDocumentationTranslator;

public class CoreSettingsTest
{
    [Fact]
    public void ToString_MultipleOutputFileLanguages()
    {
        // Arrange
        var settings = new CoreSettings
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
                       "SourceDocumentLanguage: English (United States), " +
                       "OutputDirectoryPath: path/to/output, " +
                       "OutputFileLanguages: [Japanese,Spanish]";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var settings = new CoreSettings
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
        var settings = new CoreSettings
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
