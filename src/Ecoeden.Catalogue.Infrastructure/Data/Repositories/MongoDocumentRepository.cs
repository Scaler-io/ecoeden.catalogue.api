using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Domain.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Ecoeden.Catalogue.Infrastructure.Data.Repositories;
public sealed class MongoDocumentRepository<T>(ICatalogueContext context) : IDocumentRepository<T>
    where T : EntityBase
{
    private readonly IMongoDatabase _database = context.GetDatabaseInstance();

    public async Task<IReadOnlyList<T>> GetAllAsync(string collectionName)
    {
        var collection = GetCollection(collectionName);
        return await collection.Find(itrm => true).ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id, string collectionName)
    {
        var collection = GetCollection(collectionName);
        return await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate, string collectionName)
    {
        var collection = GetCollection(collectionName);
        FilterDefinition<T> filter = Builders<T>.Filter.Where(predicate);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> GetListByPredicateAsync(Expression<Func<T, bool>> predicate, string collectionName)
    {
        var collection = GetCollection(collectionName);
        FilterDefinition<T> filter = Builders<T>.Filter.Where(predicate);
        return await collection.Find(filter).ToListAsync();
    }

    public async Task UpsertAsync(T entity, string collectionName)
    {
        var collection = GetCollection(collectionName);
        if (!string.IsNullOrEmpty(entity.Id) && await IsDocumentUpdateRequest(entity.Id, collection))
        {
            await collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        }
        else
        {
            await collection.InsertOneAsync(entity);
        }
    }

    public async Task DeleteAsync(string id, string collectionName)
    {
        var collection = GetCollection(collectionName);
        FilterDefinition<T> filter = Builders<T>.Filter.Where(x => x.Id == id);
        await collection.DeleteOneAsync(filter);
    }

    private static async Task<bool> IsDocumentUpdateRequest(string id, IMongoCollection<T> collection)
    {
        var entity = await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (entity == null) return false;
        return true;
    }

    private IMongoCollection<T> GetCollection(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }

    public async Task<bool> Exists(Expression<Func<T, bool>> predicate, string collectionName)
    {
        return await GetByPredicateAsync(predicate, collectionName) is not null;
    }
    
}
