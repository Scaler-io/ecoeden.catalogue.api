using Ecoeden.Catalogue.Domain.Sql;

namespace Ecoeden.Catalogue.Application.Contracts.Data.Sql;
public interface IBaseRepository<TEntity> where TEntity : SqlBaseEntity
{
    Task<TEntity> GetByIdAsync(object id);
    Task<IReadOnlyList<TEntity>> ListAllAsync();
    Task<TEntity> GetEntityWithSpec(ISpecification<TEntity> spec);
    Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec);
    Task<int> CountAsync(ISpecification<TEntity> spec);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
