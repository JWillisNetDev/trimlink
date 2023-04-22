using Microsoft.EntityFrameworkCore;
using trimlink.data.Models;

namespace trimlink.data.Repositories;

public class UnitOfWork : IDisposable
{
    public Guid Id { get; } = Guid.NewGuid();

    private TrimLinkDbContext _dbContext;

    public UnitOfWork(DbContextOptions<TrimLinkDbContext> options)
    {
        _dbContext = new TrimLinkDbContext(options);
    }

    public UnitOfWork(TrimLinkDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private Repository<TrimLinkDbContext, Link>? _linkRepository;
    public IRepository<TrimLinkDbContext, Link> Links
        => _linkRepository ??= new Repository<TrimLinkDbContext, Link>(_dbContext);

    public void Save() => _dbContext.SaveChanges();

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