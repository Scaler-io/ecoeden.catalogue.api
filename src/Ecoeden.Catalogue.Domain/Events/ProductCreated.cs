using Ecoeden.Catalogue.Domain.Events;
using Ecoeden.Catalogue.Domain.Models.Constants;

namespace Contracts.Events;
public class ProductCreated : GenericEvent
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string ImageFile { get; set; }
    public string Slug { get; set; }
    protected override GenericEventType GenericEventType { get; set; } = GenericEventType.ProductCreated;
}
