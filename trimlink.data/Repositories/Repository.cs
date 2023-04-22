using Microsoft.EntityFrameworkCore;
using trimlink.data.Repositories;

namespace trimlink.data.Repositories;

internal class Repository<TContext, TEntity> : IDisposable, IRepository<TContext, TEntity>
    where TContext : DbContext
    where TEntity : class
{
    private readonly TContext _dbContext;

    protected TContext DbContext => _dbContext;
    protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

    public Repository(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public TEntity? GetById(object id)
    {
        ArgumentNullException.ThrowIfNull(nameof(id));
        return DbSet.Find(id);
    }

    public IEnumerable<TEntity> GetAll()
    {
        return DbSet.ToList();
    }

    public TEntity? Find(Func<TEntity, bool> predicate)
    {
        return DbSet.FirstOrDefault(predicate);
    }

    public IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate)
    {
        return DbSet.Where(predicate).ToList();
    }

    public void Add(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public void Remove(object id)
    {
        TEntity? entity = GetById(id);
        if (entity is not null)
            DbSet.Remove(entity);
    }

    public void Update(TEntity entity)
    {
        DbSet.Entry(entity).State = EntityState.Modified;
    }

    #region Implements IDisposable
    private bool _disposed;
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _dbContext.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}