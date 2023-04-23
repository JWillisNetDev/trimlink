using trimlink.data.Models;

namespace trimlink.data.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<Link, int> Links { get; }

    void Save();
}