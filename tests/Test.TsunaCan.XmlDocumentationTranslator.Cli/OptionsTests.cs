using System.Globalization;
using Microsoft.Extensions.Logging;
using TsunaCan.XmlDocumentationTranslator.Cli;

namespace Test.TsunaCan.XmlDocumentationTranslator.Cli;

public class OptionsTests
{
    [Fact]
    public void IsValid_ReturnsTrue_WhenAllRequiredPropertiesAreSet()
    {
        // Arrange
        var options = new Options
        {
            SourceDocumentPath = "input.xml",
            OutputDirectoryPath = "./out",
            SourceDocumentLanguage = CultureInfo.GetCultureInfo("en"),
            OutputFileLanguages = [CultureInfo.GetCultureInfo("ja")],
            Token = "token123",
            ChatEndPointUrl = new Uri("https://models.inference.ai.azure.com"),
            ModelId = "model-id",
            ChunkSize = 10,
            MaxConcurrentRequests = 2,
            IsHelpRequested = false,
            LogLevel = LogLevel.Information,
        };

        // Act
        var result = options.IsValid(out var errors);

        // Assert
        Assert.True(result);
        Assert.Empty(errors);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenSourceDocumentPathIsEmpty()
    {
        // Arrange
        var options = new Options
        {
            SourceDocumentPath = string.Empty,
            OutputFileLanguages = [CultureInfo.GetCultureInfo("ja")],
            Token = "token123",
        };

        // Act
        var result = options.IsValid(out var errors);

        // Assert
        Assert.False(result);
        Assert.Contains("--source-document-path or -s is not set.", errors);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenOutputFileLanguagesIsEmpty()
    {
        // Arrange
        var options = new Options
        {
            SourceDocumentPath = "input.xml",
            OutputFileLanguages = [],
            Token = "token123",
        };

        // Act
        var result = options.IsValid(out var errors);

        // Assert
        Assert.False(result);
        Assert.Contains("--output-file-languages or -l is not set.", errors);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenTokenIsEmpty()
    {
        // Arrange
        var options = new Options
        {
            SourceDocumentPath = "input.xml",
            OutputFileLanguages = [CultureInfo.GetCultureInfo("ja")],
            Token = string.Empty,
        };

        // Act
        var result = options.IsValid(out var errors);

        // Assert
        Assert.False(result);
        Assert.Contains("--token or -t are not set.", errors);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenMultipleRequiredPropertiesAreMissing()
    {
        // Arrange
        var options = new Options();

        // Act
        var result = options.IsValid(out var errors);

        // Assert
        Assert.False(result);
        Assert.Contains("--source-document-path or -s is not set.", errors);
        Assert.Contains("--output-file-languages or -l is not set.", errors);
        Assert.Contains("--token or -t are not set.", errors);
        Assert.Equal(3, errors.Count);
    }
}
