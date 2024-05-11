using App.Metrics.Health;

namespace Ecoeden.Catalogue.Application.Contracts.HealthStatus;
public interface IHealthCheck
{
    ValueTask<HealthCheckResult> CheckIsHealthyAsync();
}
