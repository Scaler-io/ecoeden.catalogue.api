using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Category.Command.CreateCategory;

public sealed class UpsertCategoryCommand(CategoryDto category, RequestInformation requestInformation) : IRequest<Result<CategoryDto>>
{
    public string Id { get; set; } = category.Id;
    public string Name { get; set; } = category.Name;
    public RequestInformation RequestInformation { get; set;} = requestInformation;
}
