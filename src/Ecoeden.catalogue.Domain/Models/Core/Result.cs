using Ecoeden.Catalogue.Domain.Models.Enums;

namespace Ecoeden.Catalogue.Domain.Models.Core;
public sealed class Result<T>
    where T : class
{
    public T Data { get; set; }
    public bool IsSuccess { get; set; }
    public ErrorCodes? ErrorCode { get; set; }
    public string ErrorMessage { get; set; }

    public Result<T> Success(T data)
    {
        return new Result<T> { Data = data, IsSuccess = true };
    }

    public Result<T> Faliure(ErrorCodes errorCode, string errorMessage = null)
    {
        return new Result<T> { ErrorCode = errorCode, ErrorMessage = errorMessage, IsSuccess = false };
    }
}
