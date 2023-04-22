using Microsoft.EntityFrameworkCore;
using trimlink.data;
using trimlink.data.Repositories;

namespace trimlink.website;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private DbContextOptions<TrimLinkDbContext> _options;

    public UnitOfWorkFactory(DbContextOptions<TrimLinkDbContext> options)
    {
        _options = options;
    }

    public UnitOfWork CreateUnitOfWork()
    {
        UnitOfWork unitOfWork = new UnitOfWork(_options);
        return unitOfWork;
    }
}