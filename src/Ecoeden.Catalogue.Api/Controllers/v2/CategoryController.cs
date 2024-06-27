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
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using Ecoeden.Swagger.Examples.Category;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Swagger.Examples;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using FluentValidation;
using Ecoeden.Catalogue.Application.Features.Category.Query.GetAllCategories;
using Ecoeden.Catalogue.Application.Features.Category.Command.DeleteCategory;
using Ecoeden.Catalogue.Application.Features.Category.Query.GetCategory;

namespace Ecoeden.Catalogue.Api.Controllers.v2;

[ApiVersion("2")]
[Authorize]
public class CategoryController(ILogger logger, IIdentityService identityService, 
    IMediator mediator,
    IValidator<CategoryDto> categoryValidator) 
    : ApiBaseController(logger, identityService)
{
    private readonly IMediator _mediator = mediator;
    private readonly IValidator<CategoryDto> _categoryValidator = categoryValidator;

    [HttpPost("category")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "CreateOrUpdateCategory", Description = "creates new category")]
    // 200
    [ProducesResponseType(typeof(CreateOrUpdateCategoryResponseExample), (int)HttpStatusCode.OK)]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CreateOrUpdateCategoryResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerResponseExample))]
    [RequirePermission(ApiAccess.InventoryWrite)]
    public async Task<IActionResult> CreateOrUpdateCategory([FromBody] CategoryDto category)
    {
        Logger.Here().MethodEntered();
        var validationResult = _categoryValidator.Validate(category);
        if (!validationResult.IsValid)
        {
            return ProcessValidationResult(validationResult);
        }

        UpsertCategoryCommand command = new(category, RequestInformation);
        var result = await _mediator.Send(command);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("categories")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "CreateOrUpdateCategory", Description = "Gets all categories")]
    // 200
    [ProducesResponseType(typeof(List<CategoryDto>), (int)HttpStatusCode.OK)]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GetAllCategoriesExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerResponseExample))]
    [RequirePermission(ApiAccess.InventoryRead)]
    public async Task<IActionResult> GetCategories()
    {
        Logger.Here().MethodEntered();
        var getAllQuery = new GetAllCategoriesQuery();
        var result = await _mediator.Send(getAllQuery);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("category/{id}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "CreateOrUpdateCategory", Description = "Gets all categories")]
    // 200
    [ProducesResponseType(typeof(CategoryDto), (int)HttpStatusCode.OK)]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GetCategoryExample))]
    // 404
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerResponseExample))]
    [RequirePermission(ApiAccess.InventoryRead)]
    public async Task<IActionResult> GetCategoryById([FromRoute] string id)
    {
        Logger.Here().MethodEntered();
        var getQuery = new GetCategoryQuery(id);
        var result = await _mediator.Send(getQuery);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }


    [HttpDelete("category/{id}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "CreateOrUpdateCategory", Description = "Gets all categories")]
    // 404
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerResponseExample))]
    [RequirePermission(ApiAccess.InventoryWrite)]
    public async Task<IActionResult> DeleteCategory([FromRoute] string id)
    {
        Logger.Here().MethodEntered();
        var deleteQuery = new DeleteCategoryCommand(id);
        var result = await _mediator.Send(deleteQuery);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
