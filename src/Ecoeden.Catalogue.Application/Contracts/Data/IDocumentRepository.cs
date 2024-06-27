using Ecoeden.Catalogue.Domain.Entities;
using System.Linq.Expressions;

namespace Ecoeden.Catalogue.Application.Contracts.Data;
public interface IDocumentRepository<T>
    where T : EntityBase
{
    Task<bool> Exists(Expression<Func<T, bool>> predicate, string collectionName);
    Task<IReadOnlyList<T>> GetAllAsync(string collectionName);
    Task<IReadOnlyList<T>> GetListByPredicateAsync(Expression<Func<T, bool>> predicate, string collectionName);
    Task<T> GetByIdAsync(string id, string collectionName);
    Task<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate, string collectionName);
    Task UpsertAsync(T entity, string collectionName);
    Task DeleteAsync(string id, string collectionName);
}
