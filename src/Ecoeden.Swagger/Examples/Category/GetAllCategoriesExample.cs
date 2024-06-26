using Ecoeden.Catalogue.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Category;
public class GetAllCategoriesExample : IExamplesProvider<List<CategoryDto>>
{
    public List<CategoryDto> GetExamples()
    {
        return new List<CategoryDto>
        {
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Sample category 1",
                Slug = "sample-category-1",
                MetaData = new()
                {
                    CreatedAt = DateTime.Now.ToString(),
                    UpdatedAt = DateTime.Now.ToString(),
                    CreatedBy = Guid.NewGuid().ToString(),
                    UpdatedBy = Guid.NewGuid().ToString(),
                }
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Sample category 2",
                Slug = "sample-category-2",
                MetaData = new()
                {
                    CreatedAt = DateTime.Now.ToString(),
                    UpdatedAt = DateTime.Now.ToString(),
                    CreatedBy = Guid.NewGuid().ToString(),
                    UpdatedBy = Guid.NewGuid().ToString(),
                }
            }
        };
    }
}
