using Ecoeden.Catalogue.Api;
using Ecoeden.Catalogue.Api.DI;
using Ecoeden.Catalogue.Application.DI;
using Ecoeden.Catalogue.Infrastructure.DI;
using Ecoeden.Swagger;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var apiName = SwaggerConfiguration.ExtractApiNameFromEnvironmentVariable();
var apiDescription = builder.Configuration["ApiDescription"];
var apiHost = builder.Configuration["ApiOriginHost"];
var swaggerConfiguration = new SwaggerConfiguration(apiName, apiDescription, apiHost, builder.Environment.IsDevelopment());

builder.Services
    .ConfigureOptions(builder.Configuration)
    .AddApplicationServices(builder.Configuration, swaggerConfiguration)
    .AddInfraServices(builder.Configuration)
    .AddBusinessLogics();

var logger = Logging.GetLogger(builder.Configuration, builder.Environment);
builder.Host.UseSerilog(logger);

var app = builder.Build();

app.AddApplicationPipelines(app.Environment.IsDevelopment());

try
{
    await app.RunAsync();
}
finally
{
    Log.CloseAndFlush();
}
