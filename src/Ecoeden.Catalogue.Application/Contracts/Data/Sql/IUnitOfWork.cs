using Ecoeden.Catalogue.Domain.Sql;

namespace Ecoeden.Catalogue.Application.Contracts.Data.Sql;
public interface IUnitOfWork
{
    IBaseRepository<TEntity> Repository<TEntity>() where TEntity: SqlBaseEntity ;
    Task<int> Complete();
}
