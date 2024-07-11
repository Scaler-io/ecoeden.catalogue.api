using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Product.Query.GetAllProducts;
public sealed class GetAllProductsQuery : IRequest<Result<IReadOnlyList<ProductDto>>>
{
    
}
