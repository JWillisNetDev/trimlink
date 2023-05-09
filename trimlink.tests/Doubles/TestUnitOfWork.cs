namespace trimlink.tests.Doubles;

internal class TestUnitOfWork : IUnitOfWork
{
    public bool HasBeenSaved { get; private set; }

    private readonly TestLinkRepository _links;
    public IRepository<Link, int> Links => _links;

    public TestUnitOfWork(IEnumerable<Link>? linkData = null)
    {
        HasBeenSaved = false;
        _links = new TestLinkRepository(linkData);
    }

    public Task Save()
    {
        HasBeenSaved = true;
        return Task.CompletedTask;
    }
}