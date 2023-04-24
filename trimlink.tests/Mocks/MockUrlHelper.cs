using Microsoft.AspNetCore.Mvc;
using Moq;

namespace trimlink.tests.Mocks;

internal class MockUrlHelper
{
    public static Mock<IUrlHelper> GetMock()
    {
        const string baseUrl = @"https://www.trimlink.com/";
        Mock<IUrlHelper> mock = new();

        mock.Setup(uri => uri.Link(It.IsAny<string>(), It.IsAny<object>()))
            .Returns($"{baseUrl}/to/some-value");

        return mock;
    }
}