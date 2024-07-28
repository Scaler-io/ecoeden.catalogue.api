using AutoMapper;
using Ecoeden.Catalogue.Application.Contracts.Data.Sql;
using Ecoeden.Catalogue.Application.Contracts.EventBus;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Domain.Events;
using Ecoeden.Catalogue.Domain.Sql;
using MassTransit;
using Newtonsoft.Json;

namespace Ecoeden.Catalogue.Infrastructure.EventBus;
public class PublishService<T, TEvent>(IPublishEndpoint publishEndpoint, 
    IMapper mapper, 
    ILogger logger,
    IUnitOfWork unitOfWork) 
    : IPublishService<T, TEvent>
    where T : class
    where TEvent : GenericEvent
{
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly IUnitOfWork _unitOfWork = unitOfWork; 
    private readonly IBaseRepository<EventPublishHistory> _eventPublishRepository = unitOfWork.Repository<EventPublishHistory>();

    public async Task PublishAsync(T message, string correlationId, Dictionary<string, string> additionalProperties = null)
    {
        var newEvent = _mapper.Map<TEvent>(message);
        newEvent.CorrelationId = correlationId;
        newEvent.AdditionalProperties = additionalProperties;
        newEvent.AdditionalProperties.TryGetValue("applicationName", out string value);

        try
        {
            await _publishEndpoint.Publish(newEvent);

            await AddToEventStorage(newEvent, value); // store the publish history dump to sql table
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

    private async Task AddToEventStorage(TEvent eventname, string applicationName)
    {
        
        var jsonData = JsonConvert.SerializeObject(eventname);
        EventPublishHistory publishHistory = new()
        {
            CorrelationId = eventname.CorrelationId,
            CreatedAt = eventname.CreatedOn,
            UpdateAt = eventname.LastUpdatedOn,
            Data = jsonData,
            EventStatus = Domain.Models.Enums.EventStatus.Draft,
            EventType = typeof(TEvent).Name,
            FailureSource = applicationName
        };

        _eventPublishRepository.Add(publishHistory);
        await _unitOfWork.Complete();
        _logger.Here().WithCorrelationId(eventname.CorrelationId)
            .Information("Event {Event} added in {Storage} successfulyy", typeof(TEvent).Name, typeof(EventPublishHistory).Name);
    }
}
