using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Domain.Models.Core;

namespace Ecoeden.Catalogue.Application.Features.Product.Command.DeleteProduct;
public sealed class DeleteProductCommand(string productId, string correlationId) : ICommand<Result<bool>> 
{
    public string ProductId { get; private set; } = productId;
    public string CorrelationId { get; set; } = correlationId;
}
