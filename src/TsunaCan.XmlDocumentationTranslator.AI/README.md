# TsunaCan.XmlDocumentationTranslator.AI

<!-- textlint-disable -->
[![GitHub License](https://img.shields.io/github/license/tsuna-can-se/xml-documentation-translator?style=for-the-badge&color=purple)](https://github.com/tsuna-can-se/xml-documentation-translator/blob/main/LICENSE)
[![NuGet Version](https://img.shields.io/nuget/v/TsunaCan.XmlDocumentationTranslator.AI?style=for-the-badge)](https://www.nuget.org/packages/TsunaCan.XmlDocumentationTranslator.AI)

[![GitHub release (latest by date)](https://img.shields.io/github/v/release/tsuna-can-se/xml-documentation-translator?color=deep-green&label=latest%20version&style=for-the-badge)](https://github.com/tsuna-can-se/xml-documentation-translator/releases)
[![GitHub Release Date](https://img.shields.io/github/release-date/tsuna-can-se/xml-documentation-translator?color=deep-green&label=released%20in&style=for-the-badge)](https://github.com/tsuna-can-se/xml-documentation-translator/releases)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/tsuna-can-se/xml-documentation-translator?color=green&include_prereleases&label=latest%20dev%20version&style=for-the-badge)](https://github.com/tsuna-can-se/xml-documentation-translator/releases)
[![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/tsuna-can-se/xml-documentation-translator?color=green&label=released%20in&style=for-the-badge)](https://github.com/tsuna-can-se/xml-documentation-translator/releases)
<!-- textlint-enable -->

## About

`TsunaCan.XmlDocumentationTranslator.AI` uses AI to translate .NET IntelliSense documentation files in your projects.
It automates and streamlines the translation of code documentation.
This makes your codebase more accessible to global developers, regardless of any language used.

## Key Features

- AI-powered translation of XML documentation comments
- Supports multiple languages
- Easy to use with simple API
- Preserves original XML structure and formatting

## Installation

Install via NuGet:

```powershell
Install-Package TsunaCan.XmlDocumentationTranslator.AI
```

Or using the .NET CLI:

```sh
dotnet add package TsunaCan.XmlDocumentationTranslator.AI
```

## Usage

Do not use the classes in this library directly. Instead, use the `TranslationService` to translate XML documentation files.

### 1. Add Package Reference

Ensure you have added the `TsunaCan.XmlDocumentationTranslator.AI` package to your project (see Installation above).
You may also need to add `TsunaCan.XmlDocumentationTranslator.Core` as required.

### 2. Prepare Configuration

Configure `AISettings` in your `appsettings.json` or via environment variables/command-line arguments.

### 3. Example: Using from a CLI Application

Obtain `TranslationService` via dependency injection and call `ExecuteAsync`:

```csharp
using Microsoft.Extensions.Hosting;
using TsunaCan.XmlDocumentationTranslator;

var builder = Host.CreateApplicationBuilder(args);

// Add configuration (load appsettings.json, appsettings.{Environment}.json, environment variables, and command-line arguments)
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

var sourceDocumentPath = builder.Configuration.GetValue("SourceDocumentPath", string.Empty);
var sourceDocumentLanguage = builder.Configuration.GetValue<CultureInfo?>("SourceDocumentLanguage", null);
var outputDirectoryPath = builder.Configuration.GetValue("OutputDirectoryPath", string.Empty);
var outputFileLanguagesStr = builder.Configuration.GetValue("OutputFileLanguages", "ja,es");
var outputFileCultures = outputFileLanguagesStr
    .Split(',')
    .Select(c => ConvertTo(c.Trim()))
    .ToArray();

// Register services
builder.Services.AddTranslatorServices();
builder.Services.AddAITranslator(builder.Configuration.GetSection("AISettings"));

var app = builder.Build();

// Run TranslationService
var translationService = app.Services.GetRequiredService<TranslationService>();
await translationService.ExecuteAsync(sourceDocumentPath, sourceDocumentLanguage, outputDirectoryPath, outputFileCultures);
```

- Required settings (`AISettings`) should be provided via any configuration source.

## License

This project is licensed under the MIT License.
See the [LICENSE](https://github.com/tsuna-can-se/xml-documentation-translator/blob/main/LICENSE) file for details.

## Related Packages

- [TsunaCan.XmlDocumentationTranslator.Core][core-nuget]: Core library for XML documentation translation.

[core-nuget]: https://www.nuget.org/packages/TsunaCan.XmlDocumentationTranslator.Core
