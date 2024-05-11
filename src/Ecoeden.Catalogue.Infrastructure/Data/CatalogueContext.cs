using Ecoeden.Catalogue.Domain.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ecoeden.Catalogue.Infrastructure.Data;
public sealed class CatalogueContext : ICatalogueContext
{

    private readonly IMongoDatabase _mongoDatabase;

    public CatalogueContext(IOptions<MongoDbOption> mongoDbOption)
    {
        var client = new MongoClient(mongoDbOption.Value.ConnectionString);
        _mongoDatabase = client.GetDatabase(mongoDbOption.Value.Database);
    }

    public IMongoDatabase GetDatabaseInstance()
    {
        return _mongoDatabase;
    }
}
