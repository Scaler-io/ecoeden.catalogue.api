using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Ecoeden.Swagger;
using Ecoeden.Catalogue.Application.Features.HealthCheck.Queries;
using MediatR;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Swagger.Examples.HealthCheck;
using Ecoeden.Swagger.Examples;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Api.Services;

namespace Ecoeden.Catalogue.Api.Controllers.v1;

public class HealthCheckController(ILogger logger, IIdentityService identityService, IMediator mediator)
    : ApiBaseController(logger, identityService)
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("status")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "CheckApiHealth", Description = "checks for api health")]
    // 200
    [ProducesResponseType(typeof(HealthCheckDto), (int)HttpStatusCode.OK)]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(HealthCheckResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerResponseExample))]
    public async Task<IActionResult> CheckApiHealth()
    {
        Logger.Here().MethodEntered();
        var request = new GetHealthCheckQuery(RequestInformation.CorrelationId);
        var result = await _mediator.Send(request);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
