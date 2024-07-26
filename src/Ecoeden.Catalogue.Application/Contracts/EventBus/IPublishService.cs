using Ecoeden.Catalogue.Domain.Events;

namespace Ecoeden.Catalogue.Application.Contracts.EventBus;
public interface IPublishService<T, TEvent>
    where T : class
    where TEvent : GenericEvent
{
    Task PublishAsync(T message, string correlationId, object additionalProperties = null);
}
