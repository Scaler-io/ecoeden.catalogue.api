using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Ecoeden.Catalogue.Infrastructure.Data;

namespace Ecoeden.Catalogue.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication SeedDatabase(this WebApplication app,
         Action<ICatalogueContext> seeder, int? retry = 0)
    {
        int retryForAvailability = retry.Value;
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger>();
        var context = services.GetRequiredService<ICatalogueContext>();

        try
        {
            logger.Here().Information("Migrating database with context {@DbContextName}", typeof(ICatalogueContext).Name);

            seeder(context);

            logger.Here().Information("Migrated database with context {@DbContextName}", typeof(ICatalogueContext).Name);
        }
        catch (Exception ex) 
        {
            logger.Here().Error("{@ErrorCode} Migration failed. {@Message} - {@StackTrace}", ErrorCodes.OperationFailed, ex.Message, ex.StackTrace);
            if (retryForAvailability < 5)
            {
                retryForAvailability++;
                Thread.Sleep(2000);
                SeedDatabase(app, seeder, retryForAvailability);
            }
        }

        return app;
    }
}
