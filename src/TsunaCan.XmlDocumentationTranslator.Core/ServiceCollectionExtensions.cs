using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TsunaCan.XmlDocumentationTranslator.IntelliSense;

namespace TsunaCan.XmlDocumentationTranslator;

/// <summary>
///  Extension methods for adding translation core services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///  Adds AI translation services to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="configuration"><see cref="IConfiguration"/> objects.</param>
    /// <returns>Configured <see cref="IServiceCollection"/> object.</returns>
    public static IServiceCollection AddTranslatorServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<CoreSettings>()
            .Bind(configuration)
            .ValidateDataAnnotations();
        services.AddSingleton<TranslationService>();
        services.AddSingleton<IIntelliSenseDocumentManager, IntelliSenseDocumentManager>();
        return services;
    }
}
