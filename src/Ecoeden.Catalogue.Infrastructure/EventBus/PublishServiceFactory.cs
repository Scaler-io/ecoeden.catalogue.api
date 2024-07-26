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
        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider;

        return service.GetRequiredService<IPublishService<T, TEvent>>();
    }
}
