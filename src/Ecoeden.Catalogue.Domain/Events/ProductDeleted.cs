using Ecoeden.Catalogue.Domain.Events;
using Ecoeden.Catalogue.Domain.Models.Constants;

namespace Contracts.Events;
public class ProductDeleted : GenericEvent
{
    public string Id { get; set; }
    protected override GenericEventType GenericEventType { get; set; } =  GenericEventType.ProductDeleted;
}
