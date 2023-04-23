using System;
using Moq;
using trimlink.data.Repositories;

namespace trimlink.tests.Mocks;

internal class MockUnitOfWorkFactory
{
    public static Mock<IUnitOfWorkFactory> GetMock()
    {
        Mock<IUnitOfWorkFactory> mock = new();
        Mock<IUnitOfWork> mockUnitOfWork = MockUnitOfWork.GetMock();

        mock.Setup(fact => fact.CreateUnitOfWork())
            .Returns(() => mockUnitOfWork.Object);

        return mock;
    }
}

