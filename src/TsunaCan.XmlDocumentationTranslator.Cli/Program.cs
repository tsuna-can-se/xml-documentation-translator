using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TsunaCan.XmlDocumentationTranslator;
using TsunaCan.XmlDocumentationTranslator.AI;
using TsunaCan.XmlDocumentationTranslator.Cli;
using TsunaCan.XmlDocumentationTranslator.Cli.Resources;

/// <summary>
///  This is the entry point for the TsunaCan.XmlDocumentationTranslator CLI application.
///  It initializes the application, parses command line arguments,
///  configures services, and executes the translation process.
/// </summary>
public class Program
{
    private static ILogger<Program>? logger;

    /// <summary>
    ///  The main method of the application.
    ///  It sets up the host, parses command line arguments,
    ///  configures the application settings,
    ///  and executes the translation service.
    /// </summary>
    /// <param name="args">Command line arguments passed to the application.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static async Task Main(string[] args)
    {
        try
        {
            var options = new Options();
            options = CommandLineParser.ParseAndOverride(args, options);

            if (options.IsHelpRequested)
            {
                Console.WriteLine(Messages.HelpMessage);
                return;
            }

            if (!options.IsValid(out var errorMessages))
            {
                foreach (var errorMessage in errorMessages)
                {
                    Console.WriteLine(errorMessage);
                }

                Console.WriteLine();
                Console.WriteLine(Messages.HelpMessage);
                return;
            }

            var builder = Host.CreateApplicationBuilder();

            builder.Configuration
                .AddEnvironmentVariables()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [$"{nameof(AISettings)}:{nameof(AISettings.Token)}"] = options.Token,
                    [$"{nameof(AISettings)}:{nameof(AISettings.ChatEndPointUrl)}"] = options.ChatEndPointUrl?.ToString(),
                    [$"{nameof(AISettings)}:{nameof(AISettings.ChunkSize)}"] = options.ChunkSize?.ToString(),
                    [$"{nameof(AISettings)}:{nameof(AISettings.MaxConcurrentRequests)}"] = options.MaxConcurrentRequests?.ToString(),
                    [$"{nameof(AISettings)}:{nameof(AISettings.ModelId)}"] = options.ModelId,
                });

            var aiSettingsConfiguration = builder.Configuration.GetSection(nameof(AISettings));

            builder.Services.AddTranslatorServices();
            builder.Services.AddAITranslator(aiSettingsConfiguration);
            builder.Services.AddLogging(b => b.SetMinimumLevel(options.LogLevel));

            var app = builder.Build();
            logger = app.Services.GetRequiredService<ILogger<Program>>();

            var translationService = app.Services.GetRequiredService<TranslationService>();
            await translationService.ExecuteAsync(
                options.SourceDocumentPath,
                options.SourceDocumentLanguage,
                options.OutputDirectoryPath,
                options.OutputFileLanguages);
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Error occurred while executing the application.");
            Console.WriteLine(Messages.HelpMessage);
        }
    }
}
