# TsunaCan.XmlDocumentationTranslator.Core

<!-- textlint-disable -->
[![GitHub License](https://img.shields.io/github/license/tsuna-can-se/xml-documentation-translator?style=for-the-badge&color=purple)](https://github.com/tsuna-can-se/xml-documentation-translator/blob/main/LICENSE)
[![NuGet Version](https://img.shields.io/nuget/v/TsunaCan.XmlDocumentationTranslator.Core?style=for-the-badge)](https://www.nuget.org/packages/TsunaCan.XmlDocumentationTranslator.Core)

[![GitHub release (latest by date)](https://img.shields.io/github/v/release/tsuna-can-se/xml-documentation-translator?color=deep-green&label=latest%20version&style=for-the-badge)](https://github.com/tsuna-can-se/xml-documentation-translator/releases)
[![GitHub Release Date](https://img.shields.io/github/release-date/tsuna-can-se/xml-documentation-translator?color=deep-green&label=released%20in&style=for-the-badge)](https://github.com/tsuna-can-se/xml-documentation-translator/releases)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/tsuna-can-se/xml-documentation-translator?color=green&include_prereleases&label=latest%20dev%20version&style=for-the-badge)](https://github.com/tsuna-can-se/xml-documentation-translator/releases)
[![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/tsuna-can-se/xml-documentation-translator?color=green&label=released%20in&style=for-the-badge)](https://github.com/tsuna-can-se/xml-documentation-translator/releases)
<!-- textlint-enable -->

## About

This library provides the core functionality for reading, writing, and translating .NET IntelliSense documentation file.
It is designed to be used as the foundation for tools and services that automate the translation of XML documentation.
It supports multi-language output and integration with AI-based translation services.

## Key Features

- Read and write IntelliSense XML documentation files.
- Translate documentation into multiple languages.
- Pluggable translation interface for custom or AI-based translators.
- Strongly-typed settings and service registration for .NET applications.
- Logging and error handling for translation workflows.

## Installation

Install via NuGet:

```powershell
Install-Package TsunaCan.XmlDocumentationTranslator.Core
```

Or using the .NET CLI:

```sh
dotnet add package TsunaCan.XmlDocumentationTranslator.Core
```

## Usage

1. Register services in your .NET application's DI container:

   ```csharp
   services.AddTranslatorServices();
   ```

1. Register an implementation of `ITranslator` in the DI container.
   For example, you can create a simple dummy implementation for testing:

   ```csharp
   using System.Globalization;
   using TsunaCan.XmlDocumentationTranslator;
   using TsunaCan.XmlDocumentationTranslator.IntelliSense;

   public class DummyTranslator : ITranslator, IDisposable
   {
       public async Task<Dictionary<CultureInfo, IntelliSenseDocument>> TranslateAsync(
           IntelliSenseDocumentAccessor document,
           CultureInfo? sourceLanguage,
           IEnumerable<CultureInfo> targetLanguages)
       {
           var result = new Dictionary<CultureInfo, IntelliSenseDocument>();
           foreach (var targetLanguage in targetLanguages)
           {
               var translatedDocument = new IntelliSenseDocument
               {
                   Assembly = new Assembly() { Name = document.GetAssemblyName() },
               };
               var translatedXml = await this.TranslateXmlAsync(sourceLanguage, targetLanguage, document);
               translatedDocument.SetMembersInnerXml(translatedXml);
               result.Add(targetLanguage, translatedDocument);
           }
           return result;
       }

       private Task<string> TranslateXmlAsync(
           CultureInfo? sourceLanguage,
           CultureInfo targetLanguage,
           IntelliSenseDocumentAccessor document)
       {
           // translation logic here
           return Task.FromResult(string.Empty);
       }

       public void Dispose()
       {
       }
   }

   // Registration:
   services.AddSingleton<ITranslator, DummyTranslator>();
   ```

   For production use, you can use an implementation provided by the
   [`TsunaCan.XmlDocumentationTranslator.AI`][ai-nuget] package.
   This enables AI-powered translation.

1. Use `TranslationService` to execute the translation process:

   ```csharp
   string sourceDocumentPath = "path/to/document.xml";
   CultureInfo sourceDocumentLanguage = CultureInfo.GetCultureInfo("en", true);
   string outputDirectoryPath = "path/to/output";
   IEnumerable<CultureInfo> outputFileCultures = [ CultureInfo.GetCultureInfo("ja", true), CultureInfo.GetCultureInfo("es", true) ];

   var translationService = app.Services.GetRequiredService<TranslationService>();
   await translationService.ExecuteAsync(sourceDocumentPath, sourceDocumentLanguage, outputDirectoryPath, outputFileCultures);
   ```

1. The translated XML files will be output to language-specific directories.

## License

This project is licensed under the MIT License.
See the [LICENSE](https://github.com/tsuna-can-se/xml-documentation-translator/blob/main/LICENSE) file for details.

## Related Packages

- [`TsunaCan.XmlDocumentationTranslator.AI`][ai-nuget]: AI-powered translation implementation.

[ai-nuget]: https://www.nuget.org/packages/TsunaCan.XmlDocumentationTranslator.AI
