using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;

namespace Ecoeden.Catalogue.Application.Features.HealthCheck.Queries;
public sealed class GetHealthCheckQuery(string correlationId) 
    : IQuery<Result<HealthCheckDto>>
{
    public string CorrelationId { get; set; } = correlationId;
}
