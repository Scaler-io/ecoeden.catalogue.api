using Asp.Versioning.ApiExplorer;
using Ecoeden.Catalogue.Api.Middlewares;
using Ecoeden.Swagger;

namespace Ecoeden.Catalogue.Api.DI;

public static class WebApplicationExtensions
{
    public static WebApplication AddApplicationPipelines(this WebApplication app,
        bool IsDevelopment)
    {
        if (IsDevelopment)
        {
            app.UseSwagger(SwaggerConfiguration.SetupSwaggerOptions);
            app.UseSwaggerUI(option =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                SwaggerConfiguration.SetupSwaggerUiOptions(option, provider);
            });
        }

        app.UseMiddleware<CorrelationHeaderEnricher>()
            .UseMiddleware<RequestLoggerMiddleware>()
            .UseMiddleware<GlobalExceptionMiddleware>();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
