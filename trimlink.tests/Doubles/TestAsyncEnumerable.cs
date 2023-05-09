namespace trimlink.tests.Doubles;

internal class TestAsyncEnumerable<T> : IAsyncEnumerable<T>
{
    private readonly IEnumerable<T> _enumerable;

    public TestAsyncEnumerable(IEnumerable<T> data)
    {
        _enumerable = data;
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(_enumerable.GetEnumerator());
    }
}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;
    
    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }
    
    public T Current => _inner.Current;

    public ValueTask<bool> MoveNextAsync()
    {
        return ValueTask.FromResult(_inner.MoveNext());
    }

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }
}
