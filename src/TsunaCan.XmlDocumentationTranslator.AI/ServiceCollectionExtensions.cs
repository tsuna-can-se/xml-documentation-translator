using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
    /// <param name="configuration"><see cref="IConfiguration"/> objects.</param>
    /// <returns>Configured <see cref="IServiceCollection"/> object.</returns>
    public static IServiceCollection AddAITranslator(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<AISettings>()
            .Bind(configuration)
            .ValidateDataAnnotations();
        services.AddSingleton<ITranslator, AITranslator>();
        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<AISettings>>();
            return new ChatCompletionsClient(
                options.Value.ChatEndPointUrl,
                new AzureKeyCredential(options.Value.Token));
        });
        services.AddChatClient(
            provider =>
            {
                var options = provider.GetRequiredService<IOptions<AISettings>>();
                return provider.GetRequiredService<ChatCompletionsClient>()
                    .AsIChatClient(options.Value.ModelId);
            })
        .UseLogging();
        return services;
    }
}
