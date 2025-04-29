using System.Globalization;
using Microsoft.Extensions.Logging;

namespace TsunaCan.XmlDocumentationTranslator;

public class SettingsTest
{
    [Fact]
    public void ToString_ShouldReturnMaskedPartOfToken()
    {
        // Arrange
        var settings = new Settings
        {
            Token = "1234567890a",
            SourceDocumentPath = "path/to/document",
            SourceDocumentLanguage = new CultureInfo("en-US"),
            OutputDirectoryPath = "path/to/output",
            OutputFileLanguages = [new CultureInfo("ja-JP")],
            LogLevel = LogLevel.Information,
            ChatEndPointUrl = new Uri("https://example.com/"),
            ModelId = "model-id",
            ChunkSize = 1000,
        };

        // Act
        var result = settings.ToString();

        // Assert
        var expected = "Token: ******7890a, " +
                       "SourceDocumentPath: path/to/document, " +
                       "SourceDocumentLanguage: en-US, " +
                       "OutputDirectoryPath: path/to/output, " +
                       "OutputFileLanguages: [Japanese (Japan)], " +
                       "LogLevel: Information, " +
                       "ChatEndPointUrl: https://example.com/, " +
                       "ModelId: model-id, " +
                       "ChunkSize: 1000";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_ShouldReturnMaskedAllToken()
    {
        // Arrange
        var settings = new Settings
        {
            Token = "1234567890",
            SourceDocumentPath = "path/to/document",
            SourceDocumentLanguage = new CultureInfo("en-US"),
            OutputDirectoryPath = "path/to/output",
            OutputFileLanguages = [new CultureInfo("ja-JP")],
            LogLevel = LogLevel.Information,
            ChatEndPointUrl = new Uri("https://example.com/"),
            ModelId = "model-id",
            ChunkSize = 1000,
        };

        // Act
        var result = settings.ToString();

        // Assert
        var expected = "Token: **********, " +
                       "SourceDocumentPath: path/to/document, " +
                       "SourceDocumentLanguage: en-US, " +
                       "OutputDirectoryPath: path/to/output, " +
                       "OutputFileLanguages: [Japanese (Japan)], " +
                       "LogLevel: Information, " +
                       "ChatEndPointUrl: https://example.com/, " +
                       "ModelId: model-id, " +
                       "ChunkSize: 1000";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_MultipleOutputFileLanguages()
    {
        // Arrange
        var settings = new Settings
        {
            Token = "1234567890",
            SourceDocumentPath = "path/to/document",
            SourceDocumentLanguage = new CultureInfo("en-US"),
            OutputDirectoryPath = "path/to/output",
            OutputFileLanguages = [new CultureInfo("ja"), new CultureInfo("es")],
            LogLevel = LogLevel.Information,
            ChatEndPointUrl = new Uri("https://example.com/"),
            ModelId = "model-id",
            ChunkSize = 1000,
        };

        // Act
        var result = settings.ToString();

        // Assert
        var expected = "Token: **********, " +
                       "SourceDocumentPath: path/to/document, " +
                       "SourceDocumentLanguage: en-US, " +
                       "OutputDirectoryPath: path/to/output, " +
                       "OutputFileLanguages: [Japanese,Spanish], " +
                       "LogLevel: Information, " +
                       "ChatEndPointUrl: https://example.com/, " +
                       "ModelId: model-id, " +
                       "ChunkSize: 1000";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var settings = new Settings
        {
            Token = "test-token",
            SourceDocumentPath = "test-path",
            SourceDocumentLanguage = new CultureInfo("fr-FR"),
            OutputDirectoryPath = "output-path",
            OutputFileLanguages = [new CultureInfo("de-DE")],
            LogLevel = LogLevel.Debug,
            ChatEndPointUrl = new Uri("https://test.com"),
            ModelId = "test-model",
            ChunkSize = 500,
        };

        // Assert
        Assert.Equal("test-token", settings.Token);
        Assert.Equal("test-path", settings.SourceDocumentPath);
        Assert.Equal(new CultureInfo("fr-FR"), settings.SourceDocumentLanguage);
        Assert.Equal("output-path", settings.OutputDirectoryPath);
        Assert.Single(settings.OutputFileLanguages, new CultureInfo("de-DE"));
        Assert.Equal(LogLevel.Debug, settings.LogLevel);
        Assert.Equal(new Uri("https://test.com"), settings.ChatEndPointUrl);
        Assert.Equal("test-model", settings.ModelId);
    }
}
