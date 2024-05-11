using Ecoeden.Catalogue.Domain.Configurations;

namespace Ecoeden.Catalogue.Api.DI;

public static class ServiceCollectionConfigurationExtensions
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LoggingOption>(configuration.GetSection(LoggingOption.OptionName));
        services.Configure<AppConfigOption>(configuration.GetSection(AppConfigOption.OptionName));
        services.Configure<ElasticSearchOption>(configuration.GetSection(ElasticSearchOption.OptionName));
        services.Configure<IdentityGroupAccessOption>(configuration.GetSection(IdentityGroupAccessOption.OptionName));

        return services;
    }
}
