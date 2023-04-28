using Moq;
using trimlink.data.Repositories;

namespace trimlink.tests;

internal abstract class MockRepository
{
    public static Mock<IRepository<TEntity, TKey>> GetMock<TEntity, TKey>(Func<IEnumerable<TEntity>> dataFactory)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        List<TEntity> data = dataFactory().ToList();

        Mock<IRepository<TEntity, TKey>> mock = new();

        mock.Setup(rep => rep.Get(It.IsAny<TKey>()))
            .Returns((TKey key) => data.FirstOrDefault(e => e.Id.Equals(key)));

        mock.Setup(rep => rep.GetAll())
            .Returns(data);

        mock.Setup(rep => rep.Find(It.IsAny<Func<TEntity, bool>>()))
            .Returns((Func<TEntity, bool> predicate) => data.FirstOrDefault(predicate));

        mock.Setup(rep => rep.Filter(It.IsAny<Func<TEntity, bool>>()))
            .Returns((Func<TEntity, bool> predicate) => data.Where(predicate));

        mock.Setup(rep => rep.Add(It.IsAny<TEntity>()))
            .Callback((TEntity e) => data.Add(e));

        mock.Setup(rep => rep.Remove(It.IsAny<TKey>()))
            .Callback((TKey key) => data.RemoveAll(e => e.Id.Equals(key)));

        mock.Setup(rep => rep.Update(It.IsAny<TEntity>()))
            .Callback(() =>
            {
                return; // No-op
            });

        return mock;
    }
}
