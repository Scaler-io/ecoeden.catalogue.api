using Ecoeden.Catalogue.Domain.Models.Core;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Category.Command.DeleteCategory;
public sealed class DeleteCategoryCommand(string categoryId) : IRequest<Result<bool>>
{
    public string CategoryId { get; private set; } = categoryId;
}
