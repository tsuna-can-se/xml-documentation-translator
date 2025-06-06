﻿using System.Globalization;
using System.Xml.Linq;
using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Options;
using Moq;
using TsunaCan.XmlDocumentationTranslator.AI;
using TsunaCan.XmlDocumentationTranslator.IntelliSense;

namespace Test.TsunaCan.XmlDocumentationTranslator.AI;

public class AITranslatorTest(ITestOutputHelper testOutputHelper)
{
    public static IEnumerable<TheoryDataRow<IEnumerable<CultureInfo>?>> NullOrEmptyLanguages =
        [
            new TheoryDataRow<IEnumerable<CultureInfo>?>(null),
            new TheoryDataRow<IEnumerable<CultureInfo>?>([]),
        ];

    private readonly TestLoggerManager loggerManager = new(testOutputHelper);

    [Theory]
    [MemberData(nameof(NullOrEmptyLanguages))]
    public async Task TranslateAsync_TargetLanguagesIsNullOrEmpty(IEnumerable<CultureInfo>? targetLanguages)
    {
        // Arrange
        var settings = CreateDefaultSettings();
        var chatClient = new ChatClientStub(this.loggerManager.CreateLogger<ChatClientStub>());
        var logger = this.loggerManager.CreateLogger<AITranslator>();
        var translator = new AITranslator(settings, chatClient, logger);

        var xDocument = XDocument.Parse("""
            <doc>
                <assembly>
                    <name>TestAssembly</name>
                </assembly>
                <members>
                    <member name="T:TestClass">
                        <summary>Test class summary</summary>
                    </member>
                </members>
            </doc>
            """);
        var document = new IntelliSenseDocumentAccessor(xDocument);
        var sourceLanguage = new CultureInfo("en");

        // Act
        var action = () => translator.TranslateAsync(document, sourceLanguage, targetLanguages!);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>("targetLanguages", action);
        Assert.StartsWith("Target languages cannot be empty.", ex.Message);
    }

    [Fact]
    public async Task TranslateAsync_MultipleLanguageTranslation_respondInCodeBlock()
    {
        // Arrange
        var settings = CreateDefaultSettings();
        var chatClient = new ChatClientStub(this.loggerManager.CreateLogger<ChatClientStub>(), respondInCodeBlock: true);
        var logger = this.loggerManager.CreateLogger<AITranslator>();
        var translator = new AITranslator(settings, chatClient, logger);

        var xDocument = XDocument.Parse("""
            <doc>
                <assembly>
                    <name>TestAssembly</name>
                </assembly>
                <members>
                    <member name="T:TestClass">
                        <summary>Test class summary</summary>
                    </member>
                </members>
            </doc>
            """);
        var document = new IntelliSenseDocumentAccessor(xDocument);
        var sourceLanguage = new CultureInfo("en");
        IEnumerable<CultureInfo> targetLanguages = [new CultureInfo("fr"), new CultureInfo("es")];

        // Act
        var result = await translator.TranslateAsync(document, sourceLanguage, targetLanguages);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, kvp => kvp.Key.Name == "fr");
        Assert.Contains(result, kvp => kvp.Key.Name == "es");

        var fr = result[new CultureInfo("fr")];
        Assert.Equal("TestAssembly", fr.Assembly.Name);
        Assert.Equal(2, fr.MembersElement.ChildNodes.Count);
        Assert.Equal("French", fr.MembersElement.ChildNodes[1]?.InnerText); // Set by ChatClientStub.
        var es = result[new CultureInfo("es")];
        Assert.Equal("TestAssembly", es.Assembly.Name);
        Assert.Equal(2, es.MembersElement.ChildNodes.Count);
        Assert.Equal("Spanish", es.MembersElement.ChildNodes[1]?.InnerText); // Set by ChatClientStub.
    }

    [Fact]
    public async Task TranslateAsync_MultipleLanguageTranslation_respondNotInCodeBlock()
    {
        // Arrange
        var settings = CreateDefaultSettings();
        var chatClient = new ChatClientStub(this.loggerManager.CreateLogger<ChatClientStub>(), respondInCodeBlock: false);
        var logger = this.loggerManager.CreateLogger<AITranslator>();
        var translator = new AITranslator(settings, chatClient, logger);

        var xDocument = XDocument.Parse("""
            <doc>
                <assembly>
                    <name>TestAssembly</name>
                </assembly>
                <members>
                    <member name="T:TestClass">
                        <summary>Test class summary</summary>
                    </member>
                </members>
            </doc>
            """);
        var document = new IntelliSenseDocumentAccessor(xDocument);
        var sourceLanguage = new CultureInfo("en");
        IEnumerable<CultureInfo> targetLanguages = [new CultureInfo("fr"), new CultureInfo("es")];

        // Act
        var result = await translator.TranslateAsync(document, sourceLanguage, targetLanguages);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, kvp => kvp.Key.Name == "fr");
        Assert.Contains(result, kvp => kvp.Key.Name == "es");

        var fr = result[new CultureInfo("fr")];
        Assert.Equal("TestAssembly", fr.Assembly.Name);
        Assert.Equal(2, fr.MembersElement.ChildNodes.Count);
        Assert.Equal("French", fr.MembersElement.ChildNodes[1]?.InnerText); // Set by ChatClientStub.
        var es = result[new CultureInfo("es")];
        Assert.Equal("TestAssembly", es.Assembly.Name);
        Assert.Equal(2, es.MembersElement.ChildNodes.Count);
        Assert.Equal("Spanish", es.MembersElement.ChildNodes[1]?.InnerText); // Set by ChatClientStub.
    }

    [Fact]
    public async Task TranslateAsync_RespectsMaxConcurrentRequests()
    {
        // Arrange
        var chunkSize = 10;
        var maxConcurrentRequests = 6;
        var settings = CreateDefaultSettings(chunkSize, maxConcurrentRequests);
        var chatClient = new ChatClientStub(this.loggerManager.CreateLogger<ChatClientStub>(), 100);
        var logger = this.loggerManager.CreateLogger<AITranslator>();
        var translator = new AITranslator(settings, chatClient, logger);

        var xDocument = XDocument.Parse("""
            <doc>
                <assembly>
                    <name>TestAssembly</name>
                </assembly>
                <members>
                    <member name="T:TestClass1">
                        <summary>Test class summary</summary>
                    </member>
                    <member name="T:TestClass2">
                        <summary>Test class summary</summary>
                    </member>
                    <member name="T:TestClass3">
                        <summary>Test class summary</summary>
                    </member>
                    <member name="T:TestClass4">
                        <summary>Test class summary</summary>
                    </member>
                    <member name="T:TestClass5">
                        <summary>Test class summary</summary>
                    </member>
                    <member name="T:TestClass6">
                        <summary>Test class summary</summary>
                    </member>
                    <member name="T:TestClass7">
                        <summary>Test class summary</summary>
                    </member>
                    <member name="T:TestClass8">
                        <summary>Test class summary</summary>
                    </member>
                    <member name="T:TestClass9">
                        <summary>Test class summary</summary>
                    </member>
                    <member name="T:TestClass10">
                        <summary>Test class summary</summary>
                    </member>
                    <member name="T:TestClass11">
                        <summary>Test class summary</summary>
                    </member>
                    <member name="T:TestClass12">
                        <summary>Test class summary</summary>
                    </member>
                </members>
            </doc>
            """);
        var document = new IntelliSenseDocumentAccessor(xDocument);
        var sourceLanguage = new CultureInfo("en");
        IEnumerable<CultureInfo> targetLanguages = [new CultureInfo("fr"), new CultureInfo("es")];

        // Act
        _ = await translator.TranslateAsync(document, sourceLanguage, targetLanguages);

        // Assert
        Assert.Equal(maxConcurrentRequests, chatClient.MaxParallelCount); // Verify the maximum parallel count matches the setting
    }

    private static IOptions<AISettings> CreateDefaultSettings(int chunkSize = 1000, int maxConcurrentRequests = 4)
    {
        var mock = new Mock<IOptions<AISettings>>();
        mock.Setup(x => x.Value)
            .Returns(new AISettings
            {
                Token = string.Empty,
                ChatEndPointUrl = new Uri("https://example.com"),
                ModelId = "model-id",
                ChunkSize = chunkSize,
                MaxConcurrentRequests = maxConcurrentRequests,
            });
        return mock.Object;
    }
}
