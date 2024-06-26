using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Category.Query.GetAllCategories;
public class GetAllCategoriesQuery : IRequest<Result<IReadOnlyList<CategoryDto>>>
{
}
