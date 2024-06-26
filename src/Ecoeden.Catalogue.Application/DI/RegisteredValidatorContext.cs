using Ecoeden.Catalogue.Application.Validators;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Ecoeden.Catalogue.Application.DI;
internal static class RegisteredValidatorContext
{
    internal static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CategoryDto>, CategoryValidators>();
        return services;
    }
}
