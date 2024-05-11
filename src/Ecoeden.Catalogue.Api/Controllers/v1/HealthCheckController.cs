using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Ecoeden.Swagger;
using Ecoeden.Catalogue.Application.Factories;

namespace Ecoeden.Catalogue.Api.Controllers.v1;

public class HealthCheckController(ILogger logger, ICacheFactory cacheFactory) 
    : ApiBaseController(logger)
{

    private readonly ICacheFactory _cacheFactory = cacheFactory;

    [HttpGet("status")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "CheckApiHealth", Description = "checks for api health")]
    public async Task<IActionResult> CheckApiHealth()
    {
        return Ok("Hello");
    }
}
