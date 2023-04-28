using Microsoft.EntityFrameworkCore;
using Moq;

namespace trimlink.tests.Mocks;

internal static class MockDbSet
{
    public static Mock<DbSet<TEntity>> GetMock<TEntity>(IEnumerable<TEntity> data)
        where TEntity : class
    {
        IQueryable<TEntity> queryable = data.AsQueryable();

        Mock<DbSet<TEntity>> mock = new();
        mock.As<IQueryable<TEntity>>()
            .Setup(set => set.Provider)
            .Returns(queryable.Provider);

        mock.As<IQueryable<TEntity>>()
            .Setup(set => set.Expression)
            .Returns(queryable.Expression);

        mock.As<IQueryable<TEntity>>()
            .Setup(set => set.ElementType)
            .Returns(queryable.ElementType);

        mock.As<IQueryable<TEntity>>()
            .Setup(set => set.GetEnumerator())
            .Returns(() => queryable.GetEnumerator());

        return mock;
    }
}

