using Ecoeden.Catalogue.Application.Contracts.EventBus;
using Ecoeden.Catalogue.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Ecoeden.Catalogue.Infrastructure.EventBus;
public class PublishServiceFactory(IServiceProvider serviceProvider) : IPublishServiceFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IPublishService<T, TEvent> CreatePublishService<T, TEvent>()
        where T : class
        where TEvent : GenericEvent
    {
        return _serviceProvider.GetRequiredService<IPublishService<T, TEvent>>();
    }
}
