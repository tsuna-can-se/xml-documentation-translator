# TsunaCan.XmlDocumentationTranslator.Cli

<!-- textlint-disable -->
[![GitHub License](https://img.shields.io/github/license/tsuna-can-se/xml-documentation-translator?style=for-the-badge&color=purple)](https://github.com/tsuna-can-se/xml-documentation-translator/blob/main/LICENSE)
[![NuGet Version](https://img.shields.io/nuget/v/TsunaCan.XmlDocumentationTranslator.Cli?style=for-the-badge)](https://www.nuget.org/packages/TsunaCan.XmlDocumentationTranslator.Cli)

[![GitHub release (latest by date)](https://img.shields.io/github/v/release/tsuna-can-se/xml-documentation-translator?color=deep-green&label=latest%20version&style=for-the-badge)](https://github.com/tsuna-can-se/xml-documentation-translator/releases)
[![GitHub Release Date](https://img.shields.io/github/release-date/tsuna-can-se/xml-documentation-translator?color=deep-green&label=released%20in&style=for-the-badge)](https://github.com/tsuna-can-se/xml-documentation-translator/releases)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/tsuna-can-se/xml-documentation-translator?color=green&include_prereleases&label=latest%20dev%20version&style=for-the-badge)](https://github.com/tsuna-can-se/xml-documentation-translator/releases)
[![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/tsuna-can-se/xml-documentation-translator?color=green&label=released%20in&style=for-the-badge)](https://github.com/tsuna-can-se/xml-documentation-translator/releases)
<!-- textlint-enable -->

## About

`TsunaCan.XmlDocumentationTranslator.Cli` is a .NET global tool that uses AI to translate .NET IntelliSense documentation.
This tool helps translate code documentation in your projects.
It provides a command-line interface to automate and streamline the translation of code documentation.
This makes your codebase more accessible to global developers, regardless of any language used.

## Key Features

- Command-line tool for translating XML documentation files
- AI-powered translation of XML documentation comments
- Supports multiple languages
- Easy to use with command-line arguments
- Preserves original XML structure and formatting

## Installation

Install as a global .NET tool:

```sh
dotnet tool install --global TsunaCan.XmlDocumentationTranslator.Cli
```

Or install as a local tool in your project:

```sh
dotnet new tool-manifest # if you don't have this file
dotnet tool install --local TsunaCan.XmlDocumentationTranslator.Cli
```

## Usage

After installation, use the `xml-document-translate` command:

```sh
dotnet xml-document-translate --source-document-path "path/to/source.xml" --output-file-languages "ja,es,fr" --token "your-ai-token"
```

### Required Parameters

- `--source-document-path` or `-s`: Path to the source XML documentation file
- `--output-file-languages` or `-l`: Comma-separated list of target language codes (e.g., "ja,es,fr")
- `--token` or `-t`: AI service authentication token

### Optional Parameters

- `--output-directory-path`: Output directory for translated files (default: current directory)
- `--source-document-language`: Source document language code (auto-detected if not specified)
- `--chat-endpoint-url`: Custom AI service endpoint URL (default: `https://models.inference.ai.azure.com` )
- `--model-id`: AI model ID to use for translation (default: gpt-4.1-mini)
- `--chunk-size`: Size of text chunks for translation (default: 4000)
- `--max-concurrent-requests`: Maximum number of concurrent AI requests (default: 5)
- `--log-level`: Logging level (Trace, Debug, Information or other levels, default: Warning)

### Examples

Basic translation to Japanese and Spanish:

```sh
dotnet xml-document-translate -s "MyProject.xml" -l "ja,es" -t "your-token"
```

Translation with custom output directory:

```sh
dotnet xml-document-translate \
  --source-document-path "Documentation/MyProject.xml" \
  --output-directory-path "Documentation/Translations" \
  --output-file-languages "ja,es,fr" \
  --token "your-ai-token"
```

Translation with custom AI settings:

```sh
dotnet xml-document-translate \
  -s "MyProject.xml" \
  -l "ja" \
  -t "your-token" \
  --model-id "gpt-4" \
  --chunk-size 1000 \
  --max-concurrent-requests 5
```

## License

This project is licensed under the MIT License.
See the [LICENSE](https://github.com/tsuna-can-se/xml-documentation-translator/blob/main/LICENSE) file for details.

## Related Packages

- [TsunaCan.XmlDocumentationTranslator.AI][ai-nuget]: AI translation library.
- [TsunaCan.XmlDocumentationTranslator.Core][core-nuget]: Core library for XML documentation translation.

[ai-nuget]: https://www.nuget.org/packages/TsunaCan.XmlDocumentationTranslator.AI
[core-nuget]: https://www.nuget.org/packages/TsunaCan.XmlDocumentationTranslator.Core
