using Ecoeden.Catalogue.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Category;
public sealed class GetCategoryExample : IExamplesProvider<CategoryDto>
{
    public CategoryDto GetExamples()
    {
        return new()
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
        };
    }
}
