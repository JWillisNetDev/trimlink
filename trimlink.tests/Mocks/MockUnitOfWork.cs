

using Moq;
using trimlink.data.Models;
using trimlink.data.Repositories;

namespace trimlink.tests.Mocks;

internal class MockUnitOfWork
{
    public static Mock<IUnitOfWork> GetMock()
    {
        Mock<IUnitOfWork> mock = new();
        Mock<IRepository<Link, int>> mockLinkRepository = MockRepository.GetMock<Link, int>(() => new List<Link>()
        {
            new Link()
            {
                Id = 0,
                UtcDateCreated = DateTime.Parse("2023-03-26 12:00:00.000"),
                UtcDateExpires = DateTime.Parse("2024-03-26 12:00:00.000"),
                IsNeverExpires = false,
                IsMarkedForDeletion = false,
                Token = "UZuMieEQHEha",
                RedirectToUrl = "https://www.google.com/",
            },
            new Link()
            {
                Id = 1,
                UtcDateCreated = DateTime.Parse("2023-10-10 12:00:00.000"),
                UtcDateExpires = DateTime.MaxValue,
                IsNeverExpires = true,
                IsMarkedForDeletion = false,
                Token = "sVFwysRodYWB",
                RedirectToUrl = "https://www.youtube.com/",
            }
        });

        mock.Setup(work => work.Links)
            .Returns(() => mockLinkRepository.Object);

        mock.Setup(work => work.Save())
            .Callback(() =>
            {
                return;
            });

        return mock;
    }


}

