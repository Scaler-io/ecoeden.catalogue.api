using Ecoeden.Catalogue.Domain.Sql;
using System.Linq.Expressions;

namespace Ecoeden.Catalogue.Application.Contracts.Data.Sql;
public interface ISpecification<TEntity> where TEntity : SqlBaseEntity
{
    Expression<Func<TEntity, bool>> Criteria { get; }
    List<string> IncludeStrings { get; }
    Expression<Func<TEntity, object>> OrderBy { get; }
    Expression<Func<TEntity, object>> OrderByDescending { get; }
    int Take {  get; }
    int Skip {  get; }
    bool IsPagingEnabled {  get; }
}
