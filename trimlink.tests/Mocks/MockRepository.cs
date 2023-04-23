using Moq;
using trimlink.data.Repositories;

namespace trimlink.tests;

internal abstract class MockRepository<TEntity, TKey>
    where TEntity : class
{
    public static Mock<IRepository<TEntity, TKey>> GetMock(Func<IList<TEntity>> dataFactory)
    {
        IList<TEntity> data = dataFactory();

        Mock<IRepository<TEntity, TKey>> mock = new();

        // TODO We need to create an entity interface which implements a key getter that all models (with ids) implement
        //mock.Setup(repo => repo.Get(It.IsAny<TKey>()))
        //    .Returns((TKey key) => )

        return mock;
    }
}
