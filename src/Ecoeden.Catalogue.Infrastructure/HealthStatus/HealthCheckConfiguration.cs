using App.Metrics.Health;
using Ecoeden.Catalogue.Application.Contracts.HealthStatus;
using Ecoeden.Catalogue.Domain.Configurations;
using Ecoeden.Catalogue.Application.Extensions;
using Microsoft.Extensions.Options;

namespace Ecoeden.Catalogue.Infrastructure.HealthStatus;
public sealed class HealthCheckConfiguration(ILogger logger, 
    IEnumerable<IHealthCheck> healthChecks,
    IOptions<AppConfigOption> appOptions) 
    : IHealthCheckConfiguration
{
    public IRunHealthChecks HealthRunner { get; } = AppMetricsHealth
            .CreateDefaultBuilder()
            .HealthChecks
            .AddChecks(healthChecks.Select(healthCheck => CreateHealthCheck(healthCheck, logger)))
            .Build()
            .HealthCheckRunner;

    public int HealthCheckTimeOutInSeconds { get; } = appOptions.Value.HealthCheckTimeoutInSeconds;

    private static HealthCheck CreateHealthCheck(IHealthCheck health, ILogger logger)
    {
        return new HealthCheck(
            health.GetType().Name,
            async () =>
            {
                try
                {
                    return await health.CheckIsHealthyAsync();
                }
                catch (Exception ex)
                {
                    logger.Here().Error("{Exception}:", $"Health check failure {ex}");
                    return HealthCheckResult.Unhealthy();
                }
        });
    }
}
