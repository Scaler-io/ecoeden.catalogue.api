using Ecoeden.Catalogue.Application.Contracts.Security;
using Ecoeden.Catalogue.Infrastructure.Security;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ecoeden.Catalogue.Application.DI;
public static class BusinessLogicServiceExtensions
{
    public static IServiceCollection AddBusinessLogics(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddValidators();

        services.AddScoped<IPermissionMapper, PermissionMapper>();

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}
