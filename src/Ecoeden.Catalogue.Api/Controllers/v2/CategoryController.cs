using Asp.Versioning;
using Ecoeden.Catalogue.Api.Services;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Application.Features.Category.Command.CreateCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ecoeden.Catalogue.Api.Filters;
using Ecoeden.Swagger;
using Swashbuckle.AspNetCore.Annotations;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using Ecoeden.Swagger.Examples.Category;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Swagger.Examples;

namespace Ecoeden.Catalogue.Api.Controllers.v2;

[ApiVersion("2")]
[Authorize]
public class CategoryController(ILogger logger, IIdentityService identityService, IMediator mediator) 
    : ApiBaseController(logger, identityService)
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("category/{name}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "CreateOrUpdateCategory", Description = "creates new category")]
    // 200
    [ProducesResponseType(typeof(CreateOrUpdateCategoryResponseExample), (int)HttpStatusCode.OK)]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CreateOrUpdateCategoryResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerResponseExample))]
    [RequirePermission(ApiAccess.InventoryWrite)]
    public async Task<IActionResult> CreateOrUpdateCategory([FromRoute] string name)
    {
        Logger.Here().MethodEntered();
        UpsertCategoryCommand command = new(name, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
