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
            IntelliSenseDocumentPath = "path/to/document",
            IntelliSenseDocumentLocale = new CultureInfo("en-US"),
            OutputFilePath = "path/to/output",
            OutputFileLocale = new CultureInfo("ja-JP"),
            LogLevel = LogLevel.Information,
            ChatEndPointUrl = new Uri("https://example.com/"),
            ModelId = "model-id",
        };

        // Act
        var result = settings.ToString();

        // Assert
        var expected = "Token: ******7890a, " +
                       "IntelliSenseDocumentPath: path/to/document, " +
                       "IntelliSenseDocumentLocale: en-US, " +
                       "OutputFilePath: path/to/output, " +
                       "OutputFileLocale: ja-JP, " +
                       "LogLevel: Information, " +
                       "ChatEndPointUrl: https://example.com/, " +
                       "ModelId: model-id";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_ShouldReturnMaskedAllToken()
    {
        // Arrange
        var settings = new Settings
        {
            Token = "1234567890",
            IntelliSenseDocumentPath = "path/to/document",
            IntelliSenseDocumentLocale = new CultureInfo("en-US"),
            OutputFilePath = "path/to/output",
            OutputFileLocale = new CultureInfo("ja-JP"),
            LogLevel = LogLevel.Information,
            ChatEndPointUrl = new Uri("https://example.com/"),
            ModelId = "model-id",
        };

        // Act
        var result = settings.ToString();

        // Assert
        var expected = "Token: **********, " +
                       "IntelliSenseDocumentPath: path/to/document, " +
                       "IntelliSenseDocumentLocale: en-US, " +
                       "OutputFilePath: path/to/output, " +
                       "OutputFileLocale: ja-JP, " +
                       "LogLevel: Information, " +
                       "ChatEndPointUrl: https://example.com/, " +
                       "ModelId: model-id";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var settings = new Settings
        {
            Token = "test-token",
            IntelliSenseDocumentPath = "test-path",
            IntelliSenseDocumentLocale = new CultureInfo("fr-FR"),
            OutputFilePath = "output-path",
            OutputFileLocale = new CultureInfo("de-DE"),
            LogLevel = LogLevel.Debug,
            ChatEndPointUrl = new Uri("https://test.com"),
            ModelId = "test-model",
        };

        // Assert
        Assert.Equal("test-token", settings.Token);
        Assert.Equal("test-path", settings.IntelliSenseDocumentPath);
        Assert.Equal(new CultureInfo("fr-FR"), settings.IntelliSenseDocumentLocale);
        Assert.Equal("output-path", settings.OutputFilePath);
        Assert.Equal(new CultureInfo("de-DE"), settings.OutputFileLocale);
        Assert.Equal(LogLevel.Debug, settings.LogLevel);
        Assert.Equal(new Uri("https://test.com"), settings.ChatEndPointUrl);
        Assert.Equal("test-model", settings.ModelId);
    }
}
