using Ecoeden.Catalogue.Domain.Extensions;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Product;
public class ProductResponseExample : IExamplesProvider<ProductDto>
{
    public ProductDto GetExamples()
    {
        return new()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Sample product",
            Description = "This is a sample product description",
            Category = "Sample category",
            Price = 12.5M,
            Slug = "Sample product".GetSlug(),
            SKU = "Sample product".GetSku()
        };
    }
}
