using Moq;
using trimlink.core.Services;

namespace trimlink.tests.Mocks;

public class MockLinkService
{
    const string ExpectedToken = "uqsynBwWfNuv";
    const string ExpectedLongUrl = "https://www.google.com/";

    public static Mock<ILinkService> GetMock()
    {
        Mock<ILinkService> mock = new();

        int expectedId = 0;
        mock.Setup(ls => ls.GenerateShortLink(It.IsAny<string>(), out expectedId))
            .Returns(ExpectedToken);

        mock.Setup(ls => ls.GenerateShortLink(It.IsAny<string>(), It.IsAny<TimeSpan>(), out expectedId))
            .Returns(ExpectedToken);

        mock.Setup(ls => ls.GetLongUrlById(It.IsAny<int>()))
            .Returns(ExpectedLongUrl);

        mock.Setup(ls => ls.GetLongUrlByToken(It.IsAny<string>()))
            .Returns(ExpectedLongUrl);

        return mock;
    }

}

