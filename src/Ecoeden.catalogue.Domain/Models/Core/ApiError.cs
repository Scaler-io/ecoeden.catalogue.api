namespace Ecoeden.Catalogue.Domain.Models.Core;
public class ApiError
{
    public string Code { get; set; }
    public string Message { get; set; }

    public static ApiError CatgoryNameError = new() { Code = "1000", Message = "Category name is required" };
    public static ApiError ProductNameError = new() { Code = "1001", Message = "Product name is required" };
    public static ApiError ProductDescriptionRequired = new() { Code = "1002", Message = "Product description is required" };
    public static ApiError ProductDescriptionMinLength = new() { Code = "1003", Message = "Product description must be atleast 10 characters long" };
    public static ApiError ProductCategoryRequired = new() { Code = "1004", Message = "Product category is required" };
    public static ApiError ProductPriceRequired = new() { Code = "1005", Message = "Product price is required" };
    public static ApiError ProductPriceNotZero = new() { Code = "1006", Message = "Product price cannot be zero" };
}
