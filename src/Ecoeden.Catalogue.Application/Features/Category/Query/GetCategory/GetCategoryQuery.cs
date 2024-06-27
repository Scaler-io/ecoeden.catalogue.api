using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Category.Query.GetCategory;
public sealed class GetCategoryQuery(string categoryId) 
    : IRequest<Result<CategoryDto>>
{
    public string Id { get; private set; } = categoryId;
}
