using Ecoeden.Catalogue.Domain.Models.Constants;

namespace Ecoeden.Catalogue.Domain.Events;
public abstract class GenericEvent
{
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public string CorrelationId { get; set; }
    public object? AdditionalProperties { get; set; }
    protected abstract GenericEventType GenericEventType { get; set; }
}
