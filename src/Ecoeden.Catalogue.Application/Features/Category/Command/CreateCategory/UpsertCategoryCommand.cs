using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;

namespace Ecoeden.Catalogue.Application.Features.Category.Command.CreateCategory;

public sealed class UpsertCategoryCommand(CategoryDto category, RequestInformation requestInformation) : ICommand<Result<CategoryDto>>
{
    public string Id { get; set; } = category.Id;
    public string Name { get; set; } = category.Name;
    public string ImageFile { get; set; } = category.ImageFile;
    public RequestInformation RequestInformation { get; set;} = requestInformation;
}
