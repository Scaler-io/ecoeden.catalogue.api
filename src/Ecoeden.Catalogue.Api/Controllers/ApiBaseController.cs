using Ecoeden.Catalogue.Api.Extensions;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecoeden.Catalogue.Api.Controllers;

[Route("api/v{version:apiVersion}")]
[ApiController]
public class ApiBaseController(ILogger logger) : ControllerBase
{
    protected ILogger Logger { get; set; } = logger;

    protected RequestInformation RequestInformation => new RequestInformation
    {
        CorrelationId = GetOrGenerateCorelationId()
    };

    private string GetOrGenerateCorelationId() => Request?.GetRequestHeaderOrDefault("CorrelationId", $"GEN-{Guid.NewGuid()}");

    protected IActionResult OkOrFailure<T>(Result<T> result)
        where T : class
    {
        if(result == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
        if (result.IsSuccess && result.Data == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
        if (result.IsSuccess && result.Data != null) return Ok(result.Data);

        return result.ErrorCode switch
        {
            ErrorCodes.BadRequest => BadRequest(new ApiValidationResponse(result.ErrorMessage)),
            ErrorCodes.InternalServerError => InternalServerError(new ApiExceptionResponse(result.ErrorMessage)),
            ErrorCodes.NotFound => NotFound(new ApiResponse(ErrorCodes.NotFound, result.ErrorMessage)),
            ErrorCodes.Unauthorized => Unauthorized(new ApiResponse(ErrorCodes.Unauthorized, result.ErrorMessage)),
            ErrorCodes.OperationFailed => BadRequest(new ApiResponse(ErrorCodes.OperationFailed, result.ErrorMessage)),
            ErrorCodes.NotAllowed => BadRequest(new ApiResponse(ErrorCodes.NotAllowed, result.ErrorMessage)),
            _ => BadRequest(new ApiResponse(ErrorCodes.BadRequest, result.ErrorMessage))
        };
    }

    private static ObjectResult InternalServerError(ApiResponse response)
    {
        return new ObjectResult(response)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            ContentTypes =
            [
                "application/json"
            ]
        };
    }
}
