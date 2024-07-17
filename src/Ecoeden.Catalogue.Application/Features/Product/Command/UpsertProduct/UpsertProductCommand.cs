using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;

namespace Ecoeden.Catalogue.Application.Features.Product.Command.UpsertProduct;

public sealed class UpsertProductCommand(ProductDto productDto, RequestInformation requestInformation)
    : ICommand<Result<ProductDto>>
{
    public string Id { get; set; } = productDto.Id;
    public string Name { get; set; } = productDto.Name;
    public string Description { get; set; } = productDto.Description;
    public string Category { get; set; } = productDto.Category;
    public decimal Price { get; set; } = productDto.Price;
    public string ImageFile { get; set; } = productDto.ImageFile;
    public RequestInformation RequestInformation { get; set; } = requestInformation;

}
