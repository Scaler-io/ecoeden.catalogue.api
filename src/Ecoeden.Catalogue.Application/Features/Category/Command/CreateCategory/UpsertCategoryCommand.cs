using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Category.Command.CreateCategory;

public sealed class UpsertCategoryCommand(string name, RequestInformation requestInformation) : IRequest<Result<CategoryDto>>
{
    public string Name { get; set; } = name;
    public RequestInformation RequestInformation { get; set;} = requestInformation;
}
