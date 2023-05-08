namespace trimlink.core.Interfaces;

public interface IUnitOfWorkFactory
{
    IUnitOfWork CreateUnitOfWork();
}

public interface IUnitOfWorkFactory<out TUnitOfWork>
    where TUnitOfWork : IUnitOfWork
{
    TUnitOfWork CreateUnitOfWork();
}