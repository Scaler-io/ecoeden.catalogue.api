using Ecoeden.Catalogue.Application.Helpers;
using Ecoeden.Catalogue.Domain.Models.Constants;
using MongoDB.Driver;

namespace Ecoeden.Catalogue.Infrastructure.Data;
public class CatalogueSeeder
{
    public static async Task SeedAsync(ICatalogueContext context)
    {
        IMongoDatabase mongoDatabase = context.GetDatabaseInstance();

        var catalogueCollection = mongoDatabase.GetCollection<Domain.Entities.Category>(MongoDbCollectionNames.Categories);
        var productCollection = mongoDatabase.GetCollection<Domain.Entities.Product>(MongoDbCollectionNames.Products);
        
        if(await catalogueCollection.CountDocumentsAsync(_ => true) == 0)
        {
            // seed categories
            var categories = FileReaderHelper<Domain.Entities.Category>.ReadFile("categories", "./AppData");
            await catalogueCollection.InsertManyAsync(categories);
        }

        if (await productCollection.CountDocumentsAsync(_ => true) == 0)
        {
            // seed products
            var products = FileReaderHelper<Domain.Entities.Product>.ReadFile("products", "./AppData");
            await productCollection.InsertManyAsync(products);
        }
    }

}
