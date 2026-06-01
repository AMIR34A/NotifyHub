using NotifyHub.Core.BuildingBlocks.Entities;
using System.Linq.Expressions;

namespace NotifyHub.Core.Contracts.Data.Base;

public interface IBaseRepository<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : struct
{
    TEntity? Get(TId id);

    Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default);

    TEntity? GetWithAllIncludes(TId id);

    Task<TEntity?> GetWithAllIncludesAsync(TId id, CancellationToken cancellationToken = default);

    void Insert(TEntity entity);

    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Delete(TId id);

    void Delete(TEntity entity);

    void DeleteWithAllIncludes(TId id);

    bool Exists(Expression<Func<TEntity, bool>> expression);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
}