using System.Linq.Expressions;

namespace trimlink.core.Interfaces;

public interface IRepository<TEntity, in TKey>
    where TEntity : class
{
    Task<TEntity?> GetAsync(TKey id);
    IAsyncEnumerable<TEntity> GetAllAsync();
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);
    IAsyncEnumerable<TEntity> FilterAsync(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
}