using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;

namespace Ecoeden.Catalogue.Application.Features.Product.Query.GetAllProducts;
public sealed class GetAllProductsQuery : IQuery<Result<IReadOnlyList<ProductDto>>>
{
    
}
