using trimlink.data.Models;

namespace trimlink.core.Interfaces;

public interface IUnitOfWork
{
    IRepository<Link, int> Links { get; }
    Task Save();
}