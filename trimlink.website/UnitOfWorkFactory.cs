using Microsoft.EntityFrameworkCore;
using trimlink.data;
using trimlink.data.Repositories;

namespace trimlink.website;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    readonly DbContextOptions<TrimLinkDbContext> _dbContextOptions;

    public UnitOfWorkFactory(DbContextOptions<TrimLinkDbContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }

    public UnitOfWorkFactory(Action<DbContextOptionsBuilder<TrimLinkDbContext>> optionsBuilderAction)
    {
        DbContextOptionsBuilder<TrimLinkDbContext> builder = new();
        optionsBuilderAction(builder);
        _dbContextOptions = builder.Options;
    }

    public IUnitOfWork CreateUnitOfWork()
    {
        TrimLinkDbContext dbContext = new(_dbContextOptions);
        return new UnitOfWork(dbContext);
    }
}