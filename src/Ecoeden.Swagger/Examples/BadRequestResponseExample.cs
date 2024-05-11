using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples;
public class BadRequestResponseExample : IExamplesProvider<ApiResponse>
{
    public ApiResponse GetExamples()
    {
        return new ApiResponse(ErrorCodes.BadRequest)
        {
            ErrorMessage = ErrorMessages.BadRequest
        };
    }
}
