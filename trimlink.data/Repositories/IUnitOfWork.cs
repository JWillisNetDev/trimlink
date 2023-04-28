using trimlink.data.Models;

namespace trimlink.data.Repositories;

public interface IUnitOfWork
{
    IRepository<Link, int> Links { get; }

    void Save();
}