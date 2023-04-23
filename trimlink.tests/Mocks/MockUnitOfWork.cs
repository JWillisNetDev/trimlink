

using Moq;
using trimlink.data.Models;
using trimlink.data.Repositories;

namespace trimlink.tests.Mocks;

internal class MockUnitOfWork
{
    public static Mock<IUnitOfWork> GetMock()
    {
        Mock<IUnitOfWork> mock = new();
        Mock<IRepository<Link, int>> mockLinkRepository = MockLinkRepository.GetMock();

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

