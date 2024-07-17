using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Domain.Models.Core;

namespace Ecoeden.Catalogue.Application.Features.Category.Command.DeleteCategory;
public sealed class DeleteCategoryCommand(string categoryId) : ICommand<Result<bool>>
{
    public string CategoryId { get; private set; } = categoryId;
}
