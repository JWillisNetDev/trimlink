using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using trimlink.core.Interfaces;
using trimlink.data;
using trimlink.data.Models;

namespace trimlink.website;

sealed internal class UnitOfWork : IUnitOfWork, IDisposable
{
    sealed internal class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        private readonly TrimLinkDbContext _context;

        public Repository(TrimLinkDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity?> GetAsync(TKey id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public IAsyncEnumerable<TEntity> GetAllAsync()
        {
            return _context.Set<TEntity>().AsAsyncEnumerable();
        }
        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public IAsyncEnumerable<TEntity> FilterAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).AsAsyncEnumerable();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public Task RemoveAsync(TEntity entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Deleted;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TEntity entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }

    private readonly TrimLinkDbContext _context;

    public UnitOfWork(TrimLinkDbContext context)
    {
        _context = context;
        _links = new Repository<Link, int>(context);
    }
    
    private readonly Repository<Link, int> _links;
    public IRepository<Link, int> Links => _links;
    public async Task Save() => await _context.SaveChangesAsync();
    
    #region Implements IDisposable
    public void Dispose()
    {
        _context.Dispose();
    }
    #endregion
}