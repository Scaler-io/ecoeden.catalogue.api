using Ecoeden.Catalogue.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Category;
public class CategoryUpsertRequestExample : IExamplesProvider<CategoryDto>
{
    public CategoryDto GetExamples()
    {
        return new CategoryDto
        {
            Name = "Sample category",
            MetaData =
            {
                CreatedAt = DateTime.Now.ToString(),
                CreatedBy = Guid.NewGuid().ToString(),
                UpdatedAt = DateTime.Now.ToString(),
                UpdatedBy = Guid.NewGuid().ToString(),
            }
        };
    }
}
