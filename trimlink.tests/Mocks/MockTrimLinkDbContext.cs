using Microsoft.EntityFrameworkCore;
using Moq;
using trimlink.data;
using trimlink.data.Models;

namespace trimlink.tests.Mocks;

internal static class MockTrimLinkDbContext
{
    public static Mock<TrimLinkDbContext> GetMock(IEnumerable<Link>? links = null)
    {
        links ??= Enumerable.Empty<Link>();

        Mock<TrimLinkDbContext> mock = new();
        Mock<DbSet<Link>> mockLinks = MockDbSet.GetMock(links);

        mock.Setup(context => context.Links)
            .Returns(mockLinks.Object);

        mock.Setup(context => context.SaveChanges())
            .Callback(() =>
            {
            });

        return mock;
    }
}

