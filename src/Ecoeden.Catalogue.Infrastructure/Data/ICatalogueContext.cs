using MongoDB.Driver;
using StackExchange.Redis;

namespace Ecoeden.Catalogue.Infrastructure.Data;
public interface ICatalogueContext
{
    Task<bool> IsDbConnectionWorking();
    IMongoDatabase GetDatabaseInstance();
}
