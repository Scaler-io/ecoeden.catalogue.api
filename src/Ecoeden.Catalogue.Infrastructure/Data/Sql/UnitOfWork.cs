using Ecoeden.Catalogue.Application.Contracts.Data.Sql;
using Ecoeden.Catalogue.Domain.Sql;
using Ecoeden.Catalogue.Infrastructure.Data.Sql.Repositories;
using System.Collections;

namespace Ecoeden.Catalogue.Infrastructure.Data.Sql;
public class UnitOfWork(EcoedenDbContext context) : IUnitOfWork
{
    private readonly EcoedenDbContext _context = context;
    private Hashtable _repositories;

    public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : SqlBaseEntity
    {
        _repositories ??= [];
        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(BaseRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
            _repositories.Add(type, repositoryInstance);
        }

        return (IBaseRepository<TEntity>)_repositories[type];
    }

    public async Task<int> Complete() => await _context.SaveChangesAsync();

}
