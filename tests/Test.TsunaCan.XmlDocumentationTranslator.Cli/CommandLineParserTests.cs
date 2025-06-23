using System.Globalization;
using Microsoft.Extensions.Logging;
using TsunaCan.XmlDocumentationTranslator.Cli;

namespace Test.TsunaCan.XmlDocumentationTranslator.Cli;

public class CommandLineParserTests
{
    [Fact]
    public void ParseAndOverride_SetsAllPropertiesCorrectly_WhenValidArgumentsAreProvided()
    {
        // Arrange
        string[] args =
        [
            "--source-document-path", "input.xml",
            "--output-directory-path", "./out",
            "--source-document-language", "en",
            "--output-file-languages", "ja,en",
            "--token", "token123",
            "--chat-endpoint-url", "https://models.inference.ai.azure.com",
            "--model-id", "model-id",
            "--chunk-size", "10",
            "--max-concurrent-requests", "2",
            "--log-level", "Information",
        ];
        var options = new Options();

        // Act
        var result = CommandLineParser.ParseAndOverride(args, options);

        // Assert
        Assert.Equal("input.xml", result.SourceDocumentPath);
        Assert.Equal("./out", result.OutputDirectoryPath);
        Assert.Equal(CultureInfo.GetCultureInfo("en"), result.SourceDocumentLanguage);
        Assert.Equal([CultureInfo.GetCultureInfo("ja"), CultureInfo.GetCultureInfo("en")], result.OutputFileLanguages);
        Assert.Equal("token123", result.Token);
        Assert.Equal(new Uri("https://models.inference.ai.azure.com"), result.ChatEndPointUrl);
        Assert.Equal("model-id", result.ModelId);
        Assert.Equal(10, result.ChunkSize);
        Assert.Equal(2, result.MaxConcurrentRequests);
        Assert.Equal(LogLevel.Information, result.LogLevel);
        Assert.False(result.IsHelpRequested);
    }

    [Fact]
    public void ParseAndOverride_SetsIsHelpRequested_WhenHelpArgumentIsProvided()
    {
        // Arrange
        string[] args = ["--help"];
        var options = new Options();

        // Act
        var result = CommandLineParser.ParseAndOverride(args, options);

        // Assert
        Assert.True(result.IsHelpRequested);
    }

    [Fact]
    public void ParseAndOverride_ThrowsException_WhenInvalidCultureNameIsProvided()
    {
        // Arrange
        string[] args = ["--source-document-language", "invalid-culture"];
        var options = new Options();

        // Act & Assert
        var ex = Assert.Throws<CommandLineParseException>(() => CommandLineParser.ParseAndOverride(args, options));
        Assert.Contains("Invalid culture name", ex.Message);
    }

    [Fact]
    public void ParseAndOverride_ThrowsException_WhenInvalidChunkSizeIsProvided()
    {
        // Arrange
        string[] args = ["--chunk-size", "not-an-int"];
        var options = new Options();

        // Act & Assert
        var ex = Assert.Throws<CommandLineParseException>(() => CommandLineParser.ParseAndOverride(args, options));
        Assert.Contains("--chunk-size must be set to integer value", ex.Message);
    }

    [Fact]
    public void ParseAndOverride_ThrowsException_WhenInvalidMaxConcurrentRequestsIsProvided()
    {
        // Arrange
        string[] args = ["--max-concurrent-requests", "0"];
        var options = new Options();

        // Act & Assert
        var ex = Assert.Throws<CommandLineParseException>(() => CommandLineParser.ParseAndOverride(args, options));
        Assert.Contains("--max-concurrent-requests must be set to integer value", ex.Message);
    }

    [Fact]
    public void ParseAndOverride_ThrowsException_WhenInvalidLogLevelIsProvided()
    {
        // Arrange
        string[] args = ["--log-level", "NotALogLevel"];
        var options = new Options();

        // Act & Assert
        var ex = Assert.Throws<CommandLineParseException>(() => CommandLineParser.ParseAndOverride(args, options));
        Assert.Contains("--log-level must be set to Microsoft.Extensions.Logging.LogLevel value", ex.Message);
    }

    [Fact]
    public void ParseAndOverride_ThrowsException_WhenParameterNameIsUsedAsValue()
    {
        // Arrange
        string[] args = ["--source-document-path", "--token"];
        var options = new Options();

        // Act & Assert
        var ex = Assert.Throws<CommandLineParseException>(() => CommandLineParser.ParseAndOverride(args, options));
        Assert.Contains("Value was expected, but the variable", ex.Message);
    }

    [Fact]
    public void ParseAndOverride_ThrowsException_WhenInvalidChatEndpointUrlIsProvided()
    {
        // Arrange
        string[] args = ["--chat-endpoint-url", "not-a-url"];
        var options = new Options();

        // Act & Assert
        var ex = Assert.Throws<CommandLineParseException>(() => CommandLineParser.ParseAndOverride(args, options));
        Assert.Contains("--chat-endpoint-url must be set to an absolute URL", ex.Message);
    }
}
