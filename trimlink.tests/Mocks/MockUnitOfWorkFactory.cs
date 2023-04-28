using System;
using Moq;
using trimlink.data.Repositories;

namespace trimlink.tests.Mocks;

internal class MockUnitOfWorkFactory
{
    public static Mock<IUnitOfWorkFactory> GetMock()
    {
        Mock<IUnitOfWorkFactory> mock = new();

        mock.Setup(fact => fact.CreateUnitOfWork())
            .Returns(() => MockUnitOfWork.GetMock().Object);

        return mock;
    }
}

