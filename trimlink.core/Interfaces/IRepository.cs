namespace trimlink.core.Interfaces;

public interface IRepository<TEntity>
    where TEntity : class
{
    /* Getting */
    ValueTask<TEntity> Get(object id);
    IAsyncEnumerable<TEntity> GetAll();
    
    /* Predicated lookups */
    ValueTask<TEntity> Find(Func<TEntity, bool> predicate);
    IAsyncEnumerable<TEntity> Filter(Func<TEntity, bool> predicate);

    /* Modifying */
    ValueTask Add(TEntity entity);
    ValueTask Remove(TEntity entity);
    ValueTask Update(TEntity entity);
}