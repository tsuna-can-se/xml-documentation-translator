using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.AI;
using TsunaCan.XmlDocumentationTranslator;
using TsunaCan.XmlDocumentationTranslator.AI;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///  Extension methods for adding AI translation services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///  Adds AI translation services to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="settings">Settings.</param>
    /// <returns>Configured <see cref="IServiceCollection"/> object.</returns>
    public static IServiceCollection AddAITranslator(this IServiceCollection services, Settings settings)
    {
        services.AddSingleton<ITranslator, AITranslator>();
        services.AddSingleton(
            new ChatCompletionsClient(
                settings.ChatEndPointUrl,
                new AzureKeyCredential(settings.Token)));
        services.AddChatClient(
            services => services.GetRequiredService<ChatCompletionsClient>()
            .AsIChatClient(settings.ModelId))
        .UseLogging();
        return services;
    }
}
