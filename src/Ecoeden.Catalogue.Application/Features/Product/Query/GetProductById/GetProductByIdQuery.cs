using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;

namespace Ecoeden.Catalogue.Application.Features.Product.Query.GetProductById;
public sealed class GetProductByIdQuery(string id) : IQuery<Result<ProductDto>>
{
    public string Id { get; set; } = id;
}
