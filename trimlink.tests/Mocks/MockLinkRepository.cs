using Moq;
using trimlink.data.Models;
using trimlink.data.Repositories;

namespace trimlink.tests.Mocks;

internal class MockLinkRepository
{
    public static Mock<IRepository<Link, int>> GetMock()
    {
        Mock<IRepository<Link, int>> mock = new();

        List<Link> links = new()
        {
            new Link()
            {
                Id = 0,
                UtcDateCreated = DateTime.Parse("2023-03-26 12:00:00.000"),
                UtcDateExpires = DateTime.Parse("2024-03-26 12:00:00.000"),
                IsNeverExpires = false,
                IsMarkedForDeletion = false,
                ShortId = "UZuMieEQHEha",
                RedirectToUrl = "https://www.google.com/",
            },
            new Link()
            {
                Id = 1,
                UtcDateCreated = DateTime.Parse("2023-10-10 12:00:00.000"),
                UtcDateExpires = DateTime.MaxValue,
                IsNeverExpires = true,
                IsMarkedForDeletion = false,
                ShortId = "sVFwysRodYWB",
                RedirectToUrl = "https://www.youtube.com/",
            }
        };

        mock.Setup(repo => repo.Get(It.IsAny<int>()))
            .Returns((int id) => links.SingleOrDefault(link => link.Id == id));

        mock.Setup(repo => repo.GetAll())
            .Returns(() => links);

        mock.Setup(repo => repo.Find(It.IsAny<Func<Link, bool>>()))
            .Returns((Func<Link, bool> predicate) => links.FirstOrDefault(predicate));

        mock.Setup(repo => repo.Filter(It.IsAny<Func<Link, bool>>()))
            .Returns((Func<Link, bool> predicate) => links.Where(predicate));

        mock.Setup(repo => repo.Add(It.IsAny<Link>()))
            .Callback((Link link) =>
            {
                return;
            });

        mock.Setup(repo => repo.Remove(It.IsAny<int>()))
            .Callback((int id) =>
            {
                return;
            });

        mock.Setup(repo => repo.Update(It.IsAny<Link>()))
            .Callback((Link link) =>
            {
                return;
            });
        
        return mock;
    }
}