using Moq;
using trimlink.core.Services;
using trimlink.data.Models;
using trimlink.data.Repositories;
using trimlink.tests.Mocks;

namespace trimlink.tests.UnitTests;

[TestFixture]
public class LinkServiceTests
{
    class TestUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork CreateUnitOfWork()
        {
            var mock = MockUnitOfWork.GetMock();
            return mock.Object;
        }
    }

    LinkService linkService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        linkService = new LinkService(new TestUnitOfWorkFactory());
    }

    [Test]
    public void LinkService_GenerateShortLinkNoExpiration_ReturnsShortIdSuccess()
    {
        const string url = "https://www.google.com/";
        int actualId = -1;
        string actualShortId = string.Empty;

        linkService.GenerateShortLink(url, out actualId);

        Assert.That(actualId, Is.GreaterThanOrEqualTo(0));
        Assert.That(actualShortId, Is.Not.Null.Or.Empty);
    }

    [Test]
    public void LinkService_GenerateShortLinkExpires_ReturnsShortIdSuccess()
    {
        const string url = "https://www.google.com/";
        TimeSpan expiresAfter = TimeSpan.FromDays(1);
        int actualId = -1;
        string actualShortId = string.Empty;

        linkService.GenerateShortLink(url, expiresAfter, out actualId);

        Assert.That(actualId, Is.GreaterThanOrEqualTo(0));
        Assert.That(actualShortId, Is.Not.Null.Or.Empty);
    }
}