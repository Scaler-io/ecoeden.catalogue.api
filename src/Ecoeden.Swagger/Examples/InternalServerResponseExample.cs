using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples;
public class InternalServerResponseExample : IExamplesProvider<ApiExceptionResponse>
{
    public ApiExceptionResponse GetExamples()
    {
        return new ApiExceptionResponse
        {
            ErrorMessage = ErrorMessages.InternalServerError,
            StackTrace = "Dummy stack trace"
        };
    }
}
