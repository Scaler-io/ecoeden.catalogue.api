using Ecoeden.Catalogue.Domain.Events;

namespace Ecoeden.Catalogue.Application.Contracts.EventBus;
public interface IPublishServiceFactory
{
    IPublishService<T, TEvent> CreatePublishService<T, TEvent>()
        where T : class
        where TEvent : GenericEvent;

}
