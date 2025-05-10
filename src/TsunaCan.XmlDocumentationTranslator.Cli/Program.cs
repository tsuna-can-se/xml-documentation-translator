using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TsunaCan.XmlDocumentationTranslator;
using TsunaCan.XmlDocumentationTranslator.IntelliSense;

var builder = Host.CreateApplicationBuilder();
var settings = (Settings)null!; //EnvironmentReader.GetSettings();
builder.Services.AddSingleton(settings);
builder.Services.AddSingleton<TranslationService>();
builder.Services.AddSingleton<IIntelliSenseDocumentManager, IntelliSenseDocumentManager>();
builder.Services.AddAITranslator(settings);
builder.Services.AddLogging(b => b.SetMinimumLevel(settings.LogLevel));

var app = builder.Build();

var translationService = app.Services.GetRequiredService<TranslationService>();
await translationService.ExecuteAsync();
