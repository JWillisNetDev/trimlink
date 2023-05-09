using System.Collections;

namespace trimlink.tests.Doubles;

internal class TestLinkRepository : IRepository<Link, int>
{
    private readonly Dictionary<int, Link> _links;
    public IReadOnlyCollection<Link> Links => _links.Values;

    public TestLinkRepository(IEnumerable<Link>? data = null)
    {
        _links = data?.ToDictionary(link => link.Id) ?? new();
    }

    public Task<Link?> GetAsync(int id)
    {
        return Task.FromResult(_links.GetValueOrDefault(id));
    }

    public IAsyncEnumerable<Link> GetAllAsync()
    {
        return new TestAsyncEnumerable<Link>(_links.Values);
    }

    public Task<Link?> FindAsync(Func<Link, bool> predicate)
    {
        return Task.FromResult(_links.Values.FirstOrDefault(predicate));
    }

    public IAsyncEnumerable<Link> FilterAsync(Func<Link, bool> predicate)
    {
        return new TestAsyncEnumerable<Link>(_links.Values.Where(predicate));
    }

    public Task AddAsync(Link link)
    {
        link.Id = _links.Keys.Max() + 1;
        _links.Add(link.Id, link);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Link entity)
    {
        _links.Remove(entity.Id);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Link entity) => Task.CompletedTask;
}