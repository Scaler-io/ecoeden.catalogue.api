using AutoMapper;
using Ecoeden.Catalogue.Application.Contracts.EventBus;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Domain.Events;
using MassTransit;

namespace Ecoeden.Catalogue.Infrastructure.EventBus;
public class PublishService<T, TEvent>(IPublishEndpoint publishEndpoint, 
    IMapper mapper, 
    ILogger logger) 
    : IPublishService<T, TEvent>
    where T : class
    where TEvent : GenericEvent
{
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public async Task PublishAsync(T message, string correlationId, object additionalProperties = null)
    {
        var newEvent = _mapper.Map<TEvent>(message);
        newEvent.CorrelationId = correlationId;
        newEvent.AdditionalProperties = additionalProperties;

        try
        {
            await _publishEndpoint.Publish(newEvent);
            _logger.Here()
                .WithCorrelationId(correlationId)
                .Information("Successfully published {messageType} event message", typeof(TEvent).Name);
        }
        catch (Exception ex)
        {
            _logger.Here()
                .WithCorrelationId(correlationId)
                .Information("Failed to publish {messageType} event message", typeof(TEvent).Name);
        }
    }
}
