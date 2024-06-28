using App.Metrics.Health;
using Ecoeden.Catalogue.Application.Contracts.HealthStatus;
using Ecoeden.Catalogue.Infrastructure.Data;

namespace Ecoeden.Catalogue.Infrastructure.HealthStatus;
public sealed class DbHealthCheck(ICatalogueContext context) : IHealthCheck
{
    private readonly ICatalogueContext _context = context;

    public async ValueTask<HealthCheckResult> CheckIsHealthyAsync()
    {
        if(await _context.IsDbConnectionWorking())
        {
            return HealthCheckResult.Healthy();
        }

        return HealthCheckResult.Unhealthy();
    }
}
