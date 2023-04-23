using Moq;
using trimlink.core.Services;
using trimlink.data.Repositories;

namespace trimlink.tests.Mocks;

public class MockLinkService
{
    public static Mock<ILinkService> GetMock()
    {
        const string expectedShortId = "uqsynBwWfNuv";
        const string expectedLongUrl = "https://www.google.com/";

        Mock<ILinkService> mock = new();

        int expectedId = 0;
        mock.Setup(ls => ls.GenerateShortLink(It.IsAny<string>(), out expectedId))
            .Returns(expectedShortId);

        mock.Setup(ls => ls.GenerateShortLink(It.IsAny<string>(), It.IsAny<TimeSpan>(), out expectedId))
            .Returns(expectedShortId);

        mock.Setup(ls => ls.GetLongUrlById(It.IsAny<int>()))
            .Returns(expectedLongUrl);

        mock.Setup(ls => ls.GetLongUrlByShortId(It.IsAny<string>()))
            .Returns(expectedLongUrl);

        return mock;
    }

}

