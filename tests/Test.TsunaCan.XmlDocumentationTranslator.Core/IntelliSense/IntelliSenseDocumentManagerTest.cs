﻿using System.Text;
using Maris.Logging.Testing.Xunit;
using Microsoft.Extensions.Logging;

namespace TsunaCan.XmlDocumentationTranslator.IntelliSense;

public class IntelliSenseDocumentManagerTest(ITestOutputHelper testOutputHelper)
{
    private readonly TestLoggerManager loggerManager = new(testOutputHelper);

    [Fact]
    public void Read_ValidXml_ReturnsIntelliSenseDocument_HasNoMembers()
    {
        var xmlContent =
            """
            <doc>
                <assembly>
                    <name>TestAssembly</name>
                </assembly>
                <members>
                </members>
            </doc>
            """;
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, xmlContent, Encoding.UTF8);
            var logger = this.loggerManager.CreateLogger<IntelliSenseDocumentManager>();
            var manager = new IntelliSenseDocumentManager(logger);

            // Act
            var document = manager.Read(tempFilePath);

            // Assert
            Assert.NotNull(document);
            Assert.NotNull(document.GetAssemblyName());
            Assert.Equal("TestAssembly", document.GetAssemblyName());
            Assert.Empty(document.GetMembers());
        }
        finally
        {
            // Cleanup
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void Read_ValidXml_ReturnsIntelliSenseDocument_HasSingleMember()
    {
        // Arrange
        var xmlContent =
            """
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
            """;
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, xmlContent, Encoding.UTF8);
            var logger = this.loggerManager.CreateLogger<IntelliSenseDocumentManager>();
            var manager = new IntelliSenseDocumentManager(logger);

            // Act
            var document = manager.Read(tempFilePath);

            // Assert
            Assert.NotNull(document);
            Assert.Equal("TestAssembly", document.GetAssemblyName());
            var members = document.GetMembers();
            Assert.Single(members);
            Assert.Equal("<member name=\"T:TestClass\"><summary>Test class summary</summary></member>", members.First());
        }
        finally
        {
            // Cleanup
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void Read_ValidXml_ReturnsIntelliSenseDocument_HasMultipleMembers()
    {
        // Arrange
        var xmlContent =
            """
            <doc>
                <assembly>
                    <name>TestAssembly</name>
                </assembly>
                <members>
                    <member name="T:TestClass">
                        <summary>Test class summary</summary>
                    </member>
                    <member name="M:TestClass.Add(int a, int b)">
                        <summary>Add method summary.</summary>
                        <param name="a"><see cref="T:System.Int32" /> value 1.</param>
                        <param name="b"><see cref="T:System.Int32" /> value 2.</param>
                        <returns>result value.</returns>
                    </member>
                </members>
            </doc>
            """;
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, xmlContent, Encoding.UTF8);
            var logger = this.loggerManager.CreateLogger<IntelliSenseDocumentManager>();
            var manager = new IntelliSenseDocumentManager(logger);

            // Act
            var document = manager.Read(tempFilePath);

            // Assert
            Assert.NotNull(document);
            Assert.Equal("TestAssembly", document.GetAssemblyName());
            Assert.Collection(
                document.GetMembers(),
                member =>
                {
                    Assert.Equal("<member name=\"T:TestClass\"><summary>Test class summary</summary></member>", member);
                },
                member =>
                {
                    Assert.Equal("<member name=\"M:TestClass.Add(int a, int b)\"><summary>Add method summary.</summary><param name=\"a\"><see cref=\"T:System.Int32\" /> value 1.</param><param name=\"b\"><see cref=\"T:System.Int32\" /> value 2.</param><returns>result value.</returns></member>", member);
                });
        }
        finally
        {
            // Cleanup
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void Read_FileNotFound()
    {
        // Arrange
        var tempFilePath = "NOT_EXISTS_FILE";
        File.Delete(tempFilePath);
        var logger = this.loggerManager.CreateLogger<IntelliSenseDocumentManager>();
        var manager = new IntelliSenseDocumentManager(logger);

        // Act
        var action = () => manager.Read(tempFilePath);

        // Assert
        Assert.Throws<FileNotFoundException>(action);
    }

    [Fact]
    public void Read_InvalidXml()
    {
        var xmlContent =
            """
            <html>
            </html>
            """;
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, xmlContent, Encoding.UTF8);
            var logger = this.loggerManager.CreateLogger<IntelliSenseDocumentManager>();
            var manager = new IntelliSenseDocumentManager(logger);

            // Act
            var action = () => manager.Read(tempFilePath);

            // Assert
            Assert.Throws<InvalidOperationException>(action);
        }
        finally
        {
            // Cleanup
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void Read_EmptyFile()
    {
        var xmlContent = string.Empty;
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, xmlContent, Encoding.UTF8);
            var logger = this.loggerManager.CreateLogger<IntelliSenseDocumentManager>();
            var manager = new IntelliSenseDocumentManager(logger);

            // Act
            var action = () => manager.Read(tempFilePath);

            // Assert
            Assert.Throws<InvalidOperationException>(action);
        }
        finally
        {
            // Cleanup
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void Read_OutputLogOnSuccessfulFileRead()
    {
        var xmlContent =
            """
            <doc>
                <assembly>
                    <name>TestAssembly</name>
                </assembly>
                <members>
                </members>
            </doc>
            """;
        var tempFilePath = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFilePath, xmlContent, Encoding.UTF8);
            var logger = this.loggerManager.CreateLogger<IntelliSenseDocumentManager>();
            var manager = new IntelliSenseDocumentManager(logger);

            // Act
            _ = manager.Read(tempFilePath);

            // Assert
            var logCollector = this.loggerManager.LogCollector;
            Assert.Equal(2, logCollector.Count);
            var record = logCollector.LatestRecord;
            Assert.Equal(LogLevel.Information, record.Level);
            Assert.Contains(" file was loaded.", record.Message);
        }
        finally
        {
            // Cleanup
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void Write_ValidIntelliSenseDocument_CreatesXmlFile()
    {
        // Arrange
        var document = new IntelliSenseDocument
        {
            Assembly = new Assembly { Name = "TestAssembly" },
        };
        document.SetMembersInnerXml("""
            <member name="T:TestClass">
              <summary>Test class summary</summary>
            </member>
            """);
        var tempFilePath = Path.GetTempFileName();
        try
        {
            var logger = this.loggerManager.CreateLogger<IntelliSenseDocumentManager>();
            var manager = new IntelliSenseDocumentManager(logger);

            // Act
            manager.Write(tempFilePath, document);

            // Assert
            Assert.True(File.Exists(tempFilePath));
            var writtenContent = File.ReadAllText(tempFilePath, Encoding.UTF8);
            Assert.Equal(
                """
                <?xml version="1.0" encoding="utf-8"?>
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
                """,
                writtenContent);
        }
        finally
        {
            // Cleanup
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void Write_EmptyDocument()
    {
        // Arrange
        var document = new IntelliSenseDocument()
        {
            Assembly = new() { Name = string.Empty },
        };
        var tempFilePath = Path.GetTempFileName();
        try
        {
            var logger = this.loggerManager.CreateLogger<IntelliSenseDocumentManager>();
            var manager = new IntelliSenseDocumentManager(logger);

            // Act
            manager.Write(tempFilePath, document);

            // Assert
            Assert.True(File.Exists(tempFilePath));
            var writtenContent = File.ReadAllText(tempFilePath, Encoding.UTF8);
            Assert.Equal(
                """
                <?xml version="1.0" encoding="utf-8"?>
                <doc>
                    <assembly>
                        <name />
                    </assembly>
                    <members />
                </doc>
                """,
                writtenContent);
        }
        finally
        {
            // Cleanup
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void Write_OutputFilePathIsEmpty()
    {
        var logger = this.loggerManager.CreateLogger<IntelliSenseDocumentManager>();
        var manager = new IntelliSenseDocumentManager(logger);
        var document = new IntelliSenseDocument()
        {
            Assembly = new() { Name = string.Empty },
        };
        var tempFilePath = string.Empty;

        // Act
        var action = () => manager.Write(tempFilePath, document);

        // Assert
        Assert.Throws<ArgumentException>("outputFilePath", action);
    }

    [Fact]
    public void Write_OutputLogOnSuccessfulFileWrite()
    {
        // Arrange
        var document = new IntelliSenseDocument()
        {
            Assembly = new() { Name = string.Empty },
        };
        var tempFilePath = Path.GetTempFileName();
        try
        {
            var logger = this.loggerManager.CreateLogger<IntelliSenseDocumentManager>();
            var manager = new IntelliSenseDocumentManager(logger);

            // Act
            manager.Write(tempFilePath, document);

            // Assert
            var logCollector = this.loggerManager.LogCollector;
            Assert.Equal(2, logCollector.Count);
            var record = logCollector.LatestRecord;
            Assert.Equal(LogLevel.Information, record.Level);
            Assert.Equal($"Xml document {tempFilePath} was created.", record.Message);
        }
        finally
        {
            // Cleanup
            File.Delete(tempFilePath);
        }
    }

    [Fact]
    public void Write_OutputLogOnSuccessfulCreateDirectory()
    {
        // Arrange
        var document = new IntelliSenseDocument()
        {
            Assembly = new() { Name = string.Empty },
        };
        var tempFilePath = Path.Combine(Path.GetTempPath(), "DUMMY_DIR", "SUB_DIR", Path.GetRandomFileName());
        var directoryPath = Path.GetDirectoryName(tempFilePath);
        var rootDirectory = Path.GetDirectoryName(directoryPath);
        try
        {
            var logger = this.loggerManager.CreateLogger<IntelliSenseDocumentManager>();
            var manager = new IntelliSenseDocumentManager(logger);

            // Act
            manager.Write(tempFilePath, document);

            // Assert
            var logCollector = this.loggerManager.LogCollector;
            Assert.Equal(3, logCollector.Count);
            var record = logCollector.GetSnapshot()[1];
            Assert.Equal(LogLevel.Information, record.Level);
            Assert.Equal($"Directory {directoryPath} was created.", record.Message);
        }
        finally
        {
            // Cleanup
            File.Delete(tempFilePath);
            if (Directory.Exists(rootDirectory))
            {
                Directory.Delete(rootDirectory, true);
            }
        }
    }
}
