using App.Metrics.Health;
using Ecoeden.Catalogue.Application.Contracts.HealthStatus;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.HealthCheck.Queries;
public sealed class GetHealthCheckQueryHandler(ILogger logger, IHealthCheckConfiguration healthCheck) 
    : IRequestHandler<GetHealthCheckQuery, Result<HealthCheckDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IHealthCheckConfiguration _healthCheck = healthCheck;

    public async Task<Result<HealthCheckDto>> Handle(GetHealthCheckQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        var timeoutTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(_healthCheck.HealthCheckTimeOutInSeconds));
        var response = await _healthCheck.HealthRunner.ReadAsync(timeoutTokenSource.Token);

        var isHealthy = response.Status == HealthCheckStatus.Healthy;

        if (!isHealthy)
        {
            foreach (var result in response.Results)
            {
                _logger.Here().WithCorrelationId(request.CorrelationId)
                    .Error("{Message}", $"health check: {result.Name} : {result.Check.Status}");
            }
        }

        _logger.Here().WithCorrelationId(request.CorrelationId)
            .Information("{Message}", $"health check completed. Status: {response.Status}");

        var healthCheckDto = new HealthCheckDto
        {
            IsHealthy = isHealthy
        };

        _logger.Here().MethodExited();

        return Result<HealthCheckDto>.Success(healthCheckDto);
    }
}
