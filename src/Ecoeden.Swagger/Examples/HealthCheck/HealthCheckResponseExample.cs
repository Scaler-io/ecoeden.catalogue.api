using Ecoeden.Catalogue.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.HealthCheck;
public class HealthCheckResponseExample : IExamplesProvider<HealthCheckDto>
{
    public HealthCheckDto GetExamples()
    {
        return new HealthCheckDto
        {
            IsHealthy = true
        };
    }
}
