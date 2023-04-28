using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Moq;
using trimlink.data;
using trimlink.data.Models;

namespace trimlink.tests.Mocks;

internal class MockTrimLinkDbContext
{
    public static Mock<TrimLinkDbContext> GetMock([AllowNull] IEnumerable<Link> links = null)
    {
        links ??= Enumerable.Empty<Link>();

        Mock<TrimLinkDbContext> mock = new();
        Mock<DbSet<Link>> mockLinks = MockDbSet.GetMock<Link>(links);

        mock.Setup(context => context.Links)
            .Returns(mockLinks.Object);

        mock.Setup(context => context.SaveChanges())
            .Callback(() =>
            {
                return; // No-op
            });

        return mock;
    }
}

