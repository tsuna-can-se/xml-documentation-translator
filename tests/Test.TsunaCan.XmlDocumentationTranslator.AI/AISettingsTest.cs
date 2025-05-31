using TsunaCan.XmlDocumentationTranslator.AI;

namespace TsunaCan.XmlDocumentationTranslator;

public class AISettingsTest
{
    [Fact]
    public void Token_ShouldThrowArgumentNullException_WhenSetToNull()
    {
        // Arrange
        var settings = new AISettings();

        // Act & Assert
        Assert.Throws<ArgumentNullException>("Token", () => settings.Token = null!);
    }

    [Fact]
    public void ChatEndPointUrl_ShouldThrowArgumentNullException_WhenSetToNull()
    {
        // Arrange
        var settings = new AISettings();

        // Act & Assert
        Assert.Throws<ArgumentNullException>("ChatEndPointUrl", () => settings.ChatEndPointUrl = null!);
    }

    [Fact]
    public void ModelId_ShouldThrowArgumentNullException_WhenSetToNull()
    {
        // Arrange
        var settings = new AISettings();

        // Act & Assert
        Assert.Throws<ArgumentNullException>("ModelId", () => settings.ModelId = null!);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ChunkSize_ShouldThrowArgumentOutOfRangeException_WhenSetToZeroOrNegative(int value)
    {
        // Arrange
        var settings = new AISettings();

        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>("ChunkSize", () => settings.ChunkSize = value);
        Assert.StartsWith("Chunk size must be greater than zero.", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void MaxConcurrentRequests_ShouldThrowArgumentOutOfRangeException_WhenSetToZeroOrNegative(int value)
    {
        // Arrange
        var settings = new AISettings();

        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>("MaxConcurrentRequests", () => settings.MaxConcurrentRequests = value);
        Assert.StartsWith("Max concurrent requests must be greater than zero.", ex.Message);
    }

    [Fact]
    public void ToString_ShouldReturnMaskedPartOfToken()
    {
        // Arrange
        var settings = new AISettings
        {
            Token = "1234567890a",
            ChatEndPointUrl = new Uri("https://example.com/"),
            ModelId = "model-id",
            ChunkSize = 1000,
            MaxConcurrentRequests = 4,
        };

        // Act
        var result = settings.ToString();

        // Assert
        var expected = "Token: ******7890a, " +
                       "ChatEndPointUrl: https://example.com/, " +
                       "ModelId: model-id, " +
                       "ChunkSize: 1000, " +
                       "MaxConcurrentRequests: 4";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_ShouldReturnMaskedAllToken()
    {
        // Arrange
        var settings = new AISettings
        {
            Token = "1234567890",
            ChatEndPointUrl = new Uri("https://example.com/"),
            ModelId = "model-id",
            ChunkSize = 1000,
            MaxConcurrentRequests = 5,
        };

        // Act
        var result = settings.ToString();

        // Assert
        var expected = "Token: **********, " +
                       "ChatEndPointUrl: https://example.com/, " +
                       "ModelId: model-id, " +
                       "ChunkSize: 1000, " +
                       "MaxConcurrentRequests: 5";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_MultipleOutputFileLanguages()
    {
        // Arrange
        var settings = new AISettings
        {
            Token = "1234567890",
            ChatEndPointUrl = new Uri("https://example.com/"),
            ModelId = "model-id",
            ChunkSize = 1000,
            MaxConcurrentRequests = 4,
        };

        // Act
        var result = settings.ToString();

        // Assert
        var expected = "Token: **********, " +
                       "ChatEndPointUrl: https://example.com/, " +
                       "ModelId: model-id, " +
                       "ChunkSize: 1000, " +
                       "MaxConcurrentRequests: 4";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var settings = new AISettings
        {
            Token = "test-token",
            ChatEndPointUrl = new Uri("https://test.com"),
            ModelId = "test-model",
            ChunkSize = 500,
            MaxConcurrentRequests = 8,
        };

        // Assert
        Assert.Equal("test-token", settings.Token);
        Assert.Equal(new Uri("https://test.com"), settings.ChatEndPointUrl);
        Assert.Equal("test-model", settings.ModelId);
        Assert.Equal(500, settings.ChunkSize);
        Assert.Equal(8, settings.MaxConcurrentRequests);
    }
}
