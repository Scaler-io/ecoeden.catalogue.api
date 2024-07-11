using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Product.Command.UpsertProduct;

public sealed class UpsertProductCommand(ProductDto productDto, RequestInformation requestInformation)
    : IRequest<Result<ProductDto>>
{
    public string Id { get; set; } = productDto.Id;
    public string Name { get; set; } = productDto.Name;
    public string Description { get; set; } = productDto.Description;
    public string Category { get; set; } = productDto.Category;
    public decimal Price { get; set; } = productDto.Price;
    public RequestInformation RequestInformation { get; set; } = requestInformation;

}
