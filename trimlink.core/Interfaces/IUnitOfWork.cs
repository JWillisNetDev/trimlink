using trimlink.data.Models;

namespace trimlink.core.Interfaces;

public interface IUnitOfWork
{
    IRepository<Link> Links { get; }
    ValueTask Save();
}