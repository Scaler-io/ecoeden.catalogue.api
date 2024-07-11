using Ecoeden.Catalogue.Domain.Extensions;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Product;
public class ProductListResponseExample : IExamplesProvider<List<ProductDto>>
{
    public List<ProductDto> GetExamples()
    {
        return
        [
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Product 1",
                Description = "Sample product description",
                Category = "category 1",
                Price = 12,
                SKU = DomainExtensions.GetSku(),
                Slug = "Produc 1".GetSlug(),
                MetaData = new(){
                    CreatedAt = DateTime.UtcNow.ToString(),
                    UpdatedAt = DateTime.UtcNow.ToString(),
                    CreatedBy = "Default",
                    UpdatedBy = "Default"
                }
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Product 2",
                Description = "Sample product description",
                Category = "category 2",
                Price = 12,
                SKU = DomainExtensions.GetSku(),
                Slug = "Produc 2".GetSlug(),
                MetaData = new(){
                    CreatedAt = DateTime.UtcNow.ToString(),
                    UpdatedAt = DateTime.UtcNow.ToString(),
                    CreatedBy = "Default",
                    UpdatedBy = "Default"
                }
            }
        ];
    }
}
