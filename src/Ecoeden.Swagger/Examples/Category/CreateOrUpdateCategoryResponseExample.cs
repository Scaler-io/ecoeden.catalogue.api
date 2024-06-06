using Ecoeden.Catalogue.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Category;
public sealed class CreateOrUpdateCategoryResponseExample : IExamplesProvider<CategoryDto>
{
    public CategoryDto GetExamples()
    {
        return new()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Orchid",
            Slug = "orchid",
            MetaData = new()
            {
                CreatedAt = "12/11/2020",
                UpdatedAt = "12/11/2020",
                CreatedBy = "Default",
                UpdatedBy = "Default"
            }
        };
    }
}
