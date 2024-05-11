using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.HealthCheck.Queries;
public sealed class GetHealthCheckQuery(string correlationId) 
    : IRequest<Result<HealthCheckDto>>
{
    public string CorrelationId { get; set; } = correlationId;
}
