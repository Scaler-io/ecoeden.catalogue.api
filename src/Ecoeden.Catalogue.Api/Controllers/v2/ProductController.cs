using Asp.Versioning;
using Ecoeden.Catalogue.Api.Filters;
using Ecoeden.Catalogue.Api.Services;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Application.Features.Product.Command.UpsertProduct;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Ecoeden.Swagger.Examples;
using Ecoeden.Swagger;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using Ecoeden.Swagger.Examples.Product;
using Microsoft.AspNetCore.Authorization;
using Ecoeden.Catalogue.Application.Features.Product.Query.GetAllProducts;
using Ecoeden.Catalogue.Application.Features.Product.Query.GetProductById;

namespace Ecoeden.Catalogue.Api.Controllers.v2;

[ApiVersion("2")]
[Authorize]
public class ProductController(ILogger logger,
IIdentityService identityService,
IValidator<ProductDto> productValidator,
IMediator mediator)
    : ApiBaseController(logger, identityService)
{
    private readonly IValidator<ProductDto> _productValidator = productValidator;
    private readonly IMediator _mediator = mediator;

    [HttpPost("product")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "CreateOrUpdateProduct", Description = "creates or updates products")]
    // 200
    [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProductResponseExample))]
    // 400
    [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(BadRequestResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerResponseExample))]
    [RequirePermission(ApiAccess.InventoryWrite)]
    public async Task<IActionResult> CreateOrUpdateProduct([FromBody] ProductDto product)
    {
        Logger.Here().MethodEntered();

        var validationResult = _productValidator.Validate(product);
        if (IsInvalidResult(validationResult))
        {
            return ProcessValidationResult(validationResult);
        }

        UpsertProductCommand command = new(product, RequestInformation);
        var result = await _mediator.Send(command);

        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("products")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetAllProducts", Description = "Fetches all products")]
    // 200
    [ProducesResponseType(typeof(IReadOnlyList<ProductDto>), (int)HttpStatusCode.OK)]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProductListResponseExample))]
    // 400
    [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerResponseExample))]
    [RequirePermission(ApiAccess.InventoryRead)]
    public async Task<IActionResult> GetAllProducts()
    {
        Logger.Here().MethodEntered();
        var query = new GetAllProductsQuery();
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("products/{id}")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetProductById", Description = "Fetches products by its id")]
    // 200
    [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProductResponseExample))]
    // 400
    [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestResponseExample))]
    // 500
    [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerResponseExample))]
    [RequirePermission(ApiAccess.InventoryRead)]
    public async Task<IActionResult> GetProductById([FromRoute] string id)
    {
        Logger.Here().MethodEntered();
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
