using System.Globalization;
using System.Xml.Linq;
using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using TsunaCan.XmlDocumentationTranslator.IntelliSense;

namespace TsunaCan.XmlDocumentationTranslator;

public class TranslationServiceTest(ITestOutputHelper testOutputHelper)
{
    private readonly TestLoggerManager loggerManager = new(testOutputHelper);

    [Fact]
    public async Task ExecuteAsync_OutputFilesArePlacedInLanguageSpecificDirectories()
    {
        // Arrange
        var documentManagerMock = new Mock<IIntelliSenseDocumentManager>();
        var documentAccessor = new IntelliSenseDocumentAccessor(
            XDocument.Parse("""
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
                """));
        documentManagerMock
            .Setup(dm => dm.Read(It.IsAny<string>()))
            .Returns(documentAccessor);
        var translatorMock = new Mock<ITranslator>();
        translatorMock.Setup(t => t.TranslateAsync(It.IsAny<IntelliSenseDocumentAccessor>(), It.IsAny<CultureInfo>(), It.IsAny<IReadOnlyList<CultureInfo>>()))
            .ReturnsAsync(new Dictionary<CultureInfo, IntelliSenseDocument>
            {
                [new CultureInfo("fr")] = new IntelliSenseDocument() { Assembly = new Assembly() { Name = "TestAssembly" } },
                [new CultureInfo("es")] = new IntelliSenseDocument() { Assembly = new Assembly() { Name = "TestAssembly" } },
            });
        var settings = new Settings
        {
            Token = string.Empty,
            SourceDocumentPath = "source.xml",
            SourceDocumentLanguage = new CultureInfo("en"),
            OutputDirectoryPath = "output",
            OutputFileLanguages = [new CultureInfo("fr"), new CultureInfo("es")],
            ChatEndPointUrl = new Uri("https://example.com"),
            ModelId = "model-id",
            LogLevel = LogLevel.Information,
        };
        var logger = this.loggerManager.CreateLogger<TranslationService>();
        var service = new TranslationService(documentManagerMock.Object, translatorMock.Object, settings, logger);

        // Act
        await service.ExecuteAsync();

        // Assert
        Assert.Equal(2, this.loggerManager.LogCollector.Count);
        var record = this.loggerManager.LogCollector.LatestRecord;
        Assert.Equal(LogLevel.Information, record.Level);
        Assert.Contains("[output\\fr\\source.xml, output\\es\\source.xml]", record.Message);
    }
}
