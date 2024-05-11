using App.Metrics.Health;

namespace Ecoeden.Catalogue.Application.Contracts.HealthStatus;
public interface IHealthCheckConfiguration
{
    IRunHealthChecks HealthRunner { get; }
    int HealthCheckTimeOutInSeconds { get; }
}
