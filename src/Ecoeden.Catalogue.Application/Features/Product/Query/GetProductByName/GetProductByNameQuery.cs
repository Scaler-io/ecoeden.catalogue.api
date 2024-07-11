using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Product.Query.GetProductByName;
public sealed class GetProductByNameQuery(string name) : IRequest<Result<ProductDto>>
{
    public string Name { get; set; } = name;
}
