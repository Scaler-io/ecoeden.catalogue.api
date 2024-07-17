using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;

namespace Ecoeden.Catalogue.Application.Features.Category.Query.GetAllCategories;
public class GetAllCategoriesQuery : IQuery<Result<IReadOnlyList<CategoryDto>>>
{
}
