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
            .Returns(new TestAsyncQueryProvider<TEntity>(queryable.Provider));

        mock.As<IQueryable<TEntity>>()
            .Setup(set => set.Expression)
            .Returns(queryable.Expression);

        mock.As<IQueryable<TEntity>>()
            .Setup(set => set.ElementType)
            .Returns(queryable.ElementType);

        mock.As<IQueryable<TEntity>>()
            .Setup(set => set.GetEnumerator())
            .Returns(() => queryable.GetEnumerator());

        mock.As<IAsyncEnumerable<TEntity>>()
            .Setup(set => set.GetAsyncEnumerator( It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<TEntity>(data.GetEnumerator()));

        return mock;
    }
}

