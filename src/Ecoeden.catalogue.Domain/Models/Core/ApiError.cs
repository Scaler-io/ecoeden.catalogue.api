namespace Ecoeden.Catalogue.Domain.Models.Core;
public class ApiError
{
    public string Code { get; set; }
    public string Message { get; set; }

    public static ApiError CatgoryNameError = new() { Code = "1000", Message = "category name is required" };
}
