using Ecoeden.Catalogue.Application.Contracts.Data.Sql;
using Ecoeden.Catalogue.Domain.Sql;
using Ecoeden.Catalogue.Infrastructure.Data.Sql.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Ecoeden.Catalogue.Infrastructure.Data.Sql.Repositories;
public class BaseRepository<TEntity>(EcoedenDbContext context) 
    : IBaseRepository<TEntity>
    where TEntity : SqlBaseEntity
{
    private readonly EcoedenDbContext _context = context;

    public async Task<TEntity> GetByIdAsync(object id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity> GetEntityWithSpec(ISpecification<TEntity> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TEntity>> ListAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<TEntity> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity>.GetQuery(_context.Set<TEntity>().AsQueryable(), spec);
    }
}
