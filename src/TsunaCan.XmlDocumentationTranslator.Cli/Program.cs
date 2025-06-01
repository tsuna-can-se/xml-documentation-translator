using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TsunaCan.XmlDocumentationTranslator;
using TsunaCan.XmlDocumentationTranslator.AI;
using TsunaCan.XmlDocumentationTranslator.Cli;
using TsunaCan.XmlDocumentationTranslator.Cli.Resources;

var builder = Host.CreateApplicationBuilder();

builder.Configuration
    .AddEnvironmentVariables()
    .AddCommandLine(args);

var logLevel = builder.Configuration.GetValue(nameof(LogLevel), LogLevel.Information);
var sourceDocumentPath = builder.Configuration.GetValue<string>("SourceDocumentPath");
ParameterNotSetException.ThrowIfNotSet(sourceDocumentPath, "SourceDocumentPath");
var sourceDocumentLanguageStr = builder.Configuration.GetValue<string>("SourceDocumentLanguage");
var sourceDocumentLanguage = string.IsNullOrWhiteSpace(sourceDocumentLanguageStr) ? null : ConvertTo(sourceDocumentLanguageStr.Trim());
var outputDirectoryPath = builder.Configuration.GetValue("OutputDirectoryPath", string.Empty);
var outputFileLanguagesStr = builder.Configuration.GetValue<string>("OutputFileLanguages");
ParameterNotSetException.ThrowIfNotSet(outputFileLanguagesStr, "OutputFileLanguages");
var outputFileCultures = outputFileLanguagesStr
    .Split(',')
    .Select(c => ConvertTo(c.Trim()))
    .ToArray();

var aiSettingsConfiguration = builder.Configuration.GetSection(nameof(AISettings));

builder.Services.AddTranslatorServices();
builder.Services.AddAITranslator(aiSettingsConfiguration);
builder.Services.AddLogging(b => b.SetMinimumLevel(logLevel));

var app = builder.Build();

var translationService = app.Services.GetRequiredService<TranslationService>();
await translationService.ExecuteAsync(sourceDocumentPath, sourceDocumentLanguage, outputDirectoryPath, outputFileCultures);

static CultureInfo ConvertTo(string name)
{
    try
    {
        return CultureInfo.GetCultureInfo(name, true);
    }
    catch (Exception ex)
    {
        throw new ArgumentException(Messages.InvalidCultureName, nameof(name), ex);
    }
}
