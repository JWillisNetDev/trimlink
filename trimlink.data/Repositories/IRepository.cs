using Microsoft.EntityFrameworkCore;

namespace trimlink.data.Repositories;

public interface IRepository<TContext, TEntity> : IDisposable
    where TContext : DbContext
    where TEntity : class
{
    TEntity? GetById(object id);
    IEnumerable<TEntity> GetAll();

    TEntity? Find(Func<TEntity, bool> predicate);
    IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate);

    void Add(TEntity entity);
    void Remove(object id);
    void Update(TEntity entity);
}