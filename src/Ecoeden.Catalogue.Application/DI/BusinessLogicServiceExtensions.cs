using Ecoeden.Catalogue.Application.Features.HealthCheck.Queries;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ecoeden.Catalogue.Application.DI;
public static class BusinessLogicServiceExtensions
{
    public static IServiceCollection AddBusinessLogics(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}
