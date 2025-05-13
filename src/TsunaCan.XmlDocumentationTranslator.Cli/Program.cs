using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TsunaCan.XmlDocumentationTranslator;
using TsunaCan.XmlDocumentationTranslator.AI;

var builder = Host.CreateApplicationBuilder();
var settings = new Settings()
{
    SourceDocumentPath = "TsunaCan.HelloWorld.xml",
    SourceDocumentLanguage = new CultureInfo("ja"),
    OutputDirectoryPath = "output",
    OutputFileLanguages = [new CultureInfo("en")],
    LogLevel = LogLevel.Debug,
};
var aiSettings = new AISettings()
{
    ChatEndPointUrl = new Uri("https://hoge.com"),
    ModelId = "huge",
    Token = "token",
};
builder.Services.AddSingleton(settings);
builder.Services.AddAITranslator(aiSettings);
builder.Services.AddTranslatorServices(option =>
{
    option.SourceDocumentPath = settings.SourceDocumentPath;
    option.SourceDocumentLanguage = settings.SourceDocumentLanguage;
    option.OutputDirectoryPath = settings.OutputDirectoryPath;
    option.OutputFileLanguages = settings.OutputFileLanguages;
});
builder.Services.AddLogging(b => b.SetMinimumLevel(settings.LogLevel));

var app = builder.Build();

var translationService = app.Services.GetRequiredService<TranslationService>();
await translationService.ExecuteAsync();
