using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TsunaCan.XmlDocumentationTranslator;
using TsunaCan.XmlDocumentationTranslator.AI;

var builder = Host.CreateApplicationBuilder();

builder.Configuration
    .AddEnvironmentVariables()
    .AddCommandLine(args);

var logLevel = builder.Configuration.GetValue(nameof(LogLevel), LogLevel.Information);
var coreSettingsConfiguration = builder.Configuration.GetSection(nameof(CoreSettings));
var aiSettingsConfiguration = builder.Configuration.GetSection(nameof(AISettings));

builder.Services.AddTranslatorServices(coreSettingsConfiguration);
builder.Services.AddAITranslator(aiSettingsConfiguration);
builder.Services.AddLogging(b => b.SetMinimumLevel(logLevel));

var app = builder.Build();

var translationService = app.Services.GetRequiredService<TranslationService>();
await translationService.ExecuteAsync();
