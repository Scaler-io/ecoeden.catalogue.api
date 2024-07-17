using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Product.Query.GetProductById;
public sealed class GetProductByIdQuery(string id) : IRequest<Result<ProductDto>>
{
    public string Id { get; set; } = id;
}
