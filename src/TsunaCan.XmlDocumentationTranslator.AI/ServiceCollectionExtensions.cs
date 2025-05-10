using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace TsunaCan.XmlDocumentationTranslator.AI;

/// <summary>
///  Extension methods for adding AI translation services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///  Adds AI translation services to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="aiSettings">Settings.</param>
    /// <returns>Configured <see cref="IServiceCollection"/> object.</returns>
    public static IServiceCollection AddAITranslator(this IServiceCollection services, AISettings aiSettings)
    {
        services.AddOptions<AISettings>()
            .Configure(options =>
            {
                options.Token = aiSettings.Token;
                options.ChatEndPointUrl = aiSettings.ChatEndPointUrl;
                options.ModelId = aiSettings.ModelId;
                options.ChunkSize = aiSettings.ChunkSize;
                options.MaxConcurrentRequests = aiSettings.MaxConcurrentRequests;
            });
        services.AddSingleton<ITranslator, AITranslator>();
        services.AddSingleton(
            new ChatCompletionsClient(
                aiSettings.ChatEndPointUrl,
                new AzureKeyCredential(aiSettings.Token)));
        services.AddChatClient(
            services => services.GetRequiredService<ChatCompletionsClient>()
            .AsIChatClient(aiSettings.ModelId))
        .UseLogging();
        return services;
    }
}
