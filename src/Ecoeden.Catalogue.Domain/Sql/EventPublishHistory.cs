using Ecoeden.Catalogue.Domain.Models.Enums;

namespace Ecoeden.Catalogue.Domain.Sql;
public class EventPublishHistory : SqlBaseEntity
{
    public string EventType { get; set; }
    public string FailureSource { get; set; }
    public string Data { get; set; }
    public EventStatus EventStatus { get; set; }
}
