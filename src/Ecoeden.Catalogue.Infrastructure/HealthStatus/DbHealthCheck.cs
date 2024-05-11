using App.Metrics.Health;
using Ecoeden.Catalogue.Application.Contracts.HealthStatus;

namespace Ecoeden.Catalogue.Infrastructure.HealthStatus;
public sealed class DbHealthCheck : IHealthCheck
{
    public async ValueTask<HealthCheckResult> CheckIsHealthyAsync()
    {
        await Task.Yield();
        return HealthCheckResult.Healthy();
    }
}
