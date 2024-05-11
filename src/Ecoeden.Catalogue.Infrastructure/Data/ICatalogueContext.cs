using MongoDB.Driver;

namespace Ecoeden.Catalogue.Infrastructure.Data;
public interface ICatalogueContext
{
    IMongoDatabase GetDatabaseInstance();
}
