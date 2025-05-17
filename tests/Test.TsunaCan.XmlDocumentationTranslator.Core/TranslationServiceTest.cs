using System.Globalization;
using System.Xml.Linq;
using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        documentManagerMock
            .Setup(dm => dm.Write(It.IsAny<string>(), It.IsAny<IntelliSenseDocument>()))
            .Verifiable();
        var translatorMock = new Mock<ITranslator>();
        translatorMock.Setup(t => t.TranslateAsync(It.IsAny<IntelliSenseDocumentAccessor>(), It.IsAny<CultureInfo>(), It.IsAny<IReadOnlyList<CultureInfo>>()))
            .ReturnsAsync(new Dictionary<CultureInfo, IntelliSenseDocument>
            {
                [new CultureInfo("fr")] = new IntelliSenseDocument() { Assembly = new Assembly() { Name = "TestAssembly" } },
                [new CultureInfo("es")] = new IntelliSenseDocument() { Assembly = new Assembly() { Name = "TestAssembly" } },
            });
        var optionsMock = new Mock<IOptions<Settings>>();
        optionsMock.Setup(o => o.Value).Returns(new Settings
        {
            SourceDocumentPath = "source.xml",
            SourceDocumentLanguage = new CultureInfo("en"),
            OutputDirectoryPath = "output",
            OutputFileLanguages = "fr,es",
        });
        var logger = this.loggerManager.CreateLogger<TranslationService>();
        var service = new TranslationService(documentManagerMock.Object, translatorMock.Object, optionsMock.Object, logger);

        // Act
        await service.ExecuteAsync();

        // Assert
        Assert.Equal(2, this.loggerManager.LogCollector.Count);
        var record = this.loggerManager.LogCollector.LatestRecord;
        Assert.Equal(LogLevel.Information, record.Level);
        Assert.Contains($"[{Path.Combine("output", "fr", "source.xml")}, {Path.Combine("output", "es", "source.xml")}]", record.Message);

        documentManagerMock.Verify(
            dm => dm.Write(Path.Combine("output", "fr", "source.xml"), It.IsAny<IntelliSenseDocument>()),
            Times.Once);
        documentManagerMock.Verify(
            dm => dm.Write(Path.Combine("output", "es", "source.xml"), It.IsAny<IntelliSenseDocument>()),
            Times.Once);
    }
}
