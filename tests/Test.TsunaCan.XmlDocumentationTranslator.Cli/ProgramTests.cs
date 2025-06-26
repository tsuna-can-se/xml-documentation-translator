using System.Text;
using TsunaCan.XmlDocumentationTranslator.Cli.Resources;

namespace Test.TsunaCan.XmlDocumentationTranslator.Cli;

public class ProgramTests
{
    [Fact]
    public async Task Main_WritesHelpMessage_WhenHelpRequested()
    {
        // Arrange
        string[] args = ["--help"];
        var output = new StringBuilder();
        using var writer = new StringWriter(output);
        var originalOut = Console.Out;
        Console.SetOut(writer);
        try
        {
            // Act
            await Program.Main(args);
            writer.Flush();

            // Assert
            var result = output.ToString();
            Assert.Contains(Messages.HelpMessage, result);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public async Task Main_WritesValidationErrors_WhenOptionsInvalid()
    {
        // Arrange: 必須引数なし
        string[] args = [];
        var output = new StringBuilder();
        using var writer = new StringWriter(output);
        var originalOut = Console.Out;
        Console.SetOut(writer);
        try
        {
            // Act
            await Program.Main(args);
            writer.Flush();

            // Assert
            var result = output.ToString();
            Assert.Contains("--source-document-path or -s is not set.", result);
            Assert.Contains("--output-file-languages or -l is not set.", result);
            Assert.Contains("--token or -t are not set.", result);
            Assert.Contains(Messages.HelpMessage, result);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public async Task Main_LogsErrorAndWritesHelp_WhenExceptionThrown()
    {
        // Arrange: 無効な引数で例外を発生させる
        string[] args = ["--source-document-language", "invalid-culture"];
        var output = new StringBuilder();
        using var writer = new StringWriter(output);
        var originalOut = Console.Out;
        Console.SetOut(writer);
        try
        {
            // Act
            await Program.Main(args);
            writer.Flush();

            // Assert
            var result = output.ToString();
            Assert.Contains(Messages.HelpMessage, result);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}
