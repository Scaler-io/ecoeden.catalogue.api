using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;

namespace Ecoeden.Catalogue.Application.Features.Category.Query.GetCategory;
public sealed class GetCategoryQuery(string categoryId) 
    : IQuery<Result<CategoryDto>>
{
    public string Id { get; private set; } = categoryId;
}
