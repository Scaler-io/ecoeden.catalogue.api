using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Product.Query.GetProductsByCategory;
public sealed class GetProductsByCategoryQuery(string category) : IRequest<Result<IReadOnlyList<ProductDto>>>
{
    public string Category { get; set; } = category;
}
