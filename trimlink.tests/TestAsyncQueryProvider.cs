using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace trimlink.tests;

internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;
    public TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }
    public IQueryable CreateQuery(Expression expression)
        => new TestAsyncEnumerable<TEntity>(expression);
    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => new TestAsyncEnumerable<TElement>(expression);
    public object? Execute(Expression expression)
        => _inner.Execute(expression);
    public TResult Execute<TResult>(Expression expression)
        => _inner.Execute<TResult>(expression);
    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        => new TestAsyncEnumerable<TResult>(expression);
    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        // Thank you to https://stackoverflow.com/a/58314109 for this elegant, reflection based solution.
        // Due to a change in EntityFramework that changed the return type of this interface's method from Task<TResult> to TResult,
        // we must use some clever reflection magic to construct a return type that's still a task of TResult (Task is now implied, but we must be explicit)
        Type resultType = typeof(TResult).GetGenericArguments()[0];
        object? executionResult = (this as IQueryProvider).Execute(expression);
        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(resultType)
            .Invoke(null, new[] { executionResult })!;
    }
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
    {
    }
    public TestAsyncEnumerable(Expression expression) : base(expression)
    {
    }
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;
    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }
    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }
    public ValueTask<bool> MoveNextAsync()
        => ValueTask.FromResult(_inner.MoveNext());
    public T Current => _inner.Current;
}