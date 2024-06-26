using Ecoeden.Catalogue.Domain.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ecoeden.Catalogue.Infrastructure.Data;
public sealed class CatalogueContext(IOptions<MongoDbOption> mongoDbOption, MongoClient client) 
    : ICatalogueContext
{
    private readonly IMongoDatabase _mongoDatabase = client.GetDatabase(mongoDbOption.Value.Database);

    public IMongoDatabase GetDatabaseInstance()
    {
        return _mongoDatabase;
    }
}
