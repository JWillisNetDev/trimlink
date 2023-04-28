namespace trimlink.data.Repositories;

public interface IRepository<TEntity, in TKey> : IDisposable
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    TEntity? Get(TKey id);
    IEnumerable<TEntity> GetAll();

    TEntity? Find(Func<TEntity, bool> predicate);
    IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate);

    void Add(TEntity entity);
    void Remove(TKey id);
    void Update(TEntity entity);
}