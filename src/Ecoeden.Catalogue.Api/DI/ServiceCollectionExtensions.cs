using Asp.Versioning.ApiExplorer;
using Ecoeden.Catalogue.Api.Filters;
using Ecoeden.Catalogue.Api.Middlewares;
using Ecoeden.Catalogue.Api.Services;
using Ecoeden.Catalogue.Domain.Configurations;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Ecoeden.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Catalogue.Api.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, 
        IConfiguration configuration,
        SwaggerConfiguration swaggerConfiguration)
    {
        var env = services.BuildServiceProvider().GetRequiredService<IWebHostEnvironment>();
        var logger = Logging.GetLogger(configuration, env);

        services.AddSingleton(logger);

        services.AddControllers()
            .AddNewtonsoftJson(configuration =>
            {
                configuration.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                configuration.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                configuration.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        services.AddHttpContextAccessor();
        services.AddSingleton<IIdentityService, IdentityService>();

        services.AddTransient<CorrelationHeaderEnricher>();
        services.AddTransient<RequestLoggerMiddleware>();
        services.AddTransient<GlobalExceptionMiddleware>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerExamples();
        services.AddSwaggerGen(options =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            swaggerConfiguration.SetupSwaggerGenService(options, provider);
        });


        // setup api versioning
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = Asp.Versioning.ApiVersion.Default;
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        // handles api's default error validation model
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = HandleFrameworkValidationFailure();
        });

        var identityGroupAccess = configuration
        .GetSection("IdentityGroupAccess")
            .Get<IdentityGroupAccessOption>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.Audience = identityGroupAccess.Audience;
                options.Authority = identityGroupAccess.Authority;
                options.TokenValidationParameters = new()
                {
                    ValidIssuer = identityGroupAccess.Authority,
                    ValidAudience = identityGroupAccess.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }

    private static Func<ActionContext, IActionResult> HandleFrameworkValidationFailure()
    {
        return context =>
        {
            var errors = context.ModelState
                        .Where(m => m.Value.Errors.Count > 0)
                        .ToList();

            var validationError = new ApiValidationResponse
            {
                Errors = []
            };

            foreach (var error in errors)
            {
                var fieldLevelError = new FieldLevelError
                {
                    Code = ErrorCodes.BadRequest.ToString(),
                    Field = error.Key,
                    Message = error.Value.Errors?.First().ErrorMessage
                };
                validationError.Errors.Add(fieldLevelError);
            }

            return new BadRequestObjectResult(validationError);
        };
    }
}
