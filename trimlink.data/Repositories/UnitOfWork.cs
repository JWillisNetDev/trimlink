using Microsoft.EntityFrameworkCore;
using trimlink.data.Models;

namespace trimlink.data.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private TrimLinkDbContext _dbContext;

    public UnitOfWork(TrimLinkDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private Repository<TrimLinkDbContext, Link, int>? _linkRepository;
    public IRepository<Link, int> Links => _linkRepository ??= new Repository<TrimLinkDbContext, Link, int>(_dbContext);

    public void Save()
    {
        _dbContext.SaveChanges();
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