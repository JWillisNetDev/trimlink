namespace trimlink.core.Interfaces;

public interface IUnitOfWorkFactory
{
    Task<IUnitOfWork> CreateUnitOfWork();
}

public interface IUnitOfWorkFactory<TUnitOfWork>
    where TUnitOfWork : IUnitOfWork
{
    Task<TUnitOfWork> CreateUnitOfWork();
}