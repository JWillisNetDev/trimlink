using Moq;
using trimlink.core.Services;

namespace trimlink.tests.Mocks;

internal static class MockLinkService
{
    public const string ExpectedToken = "uqsynBwWfNuv";
    public const string ExpectedLongUrl = "https://www.google.com/";

    public static Mock<ILinkService> GetMock()
    {
        Mock<ILinkService> mock = new();

        mock.Setup(ls => ls.GenerateShortLink(It.IsAny<string>(), null))
            .Returns(Task.FromResult(ExpectedToken));

        mock.Setup(ls => ls.GenerateShortLink(It.IsAny<string>(), It.IsAny<TimeSpan>()))
            .Returns(Task.FromResult(ExpectedToken));

        mock.Setup(ls => ls.GetLongUrlById(It.IsAny<int>()))
            .Returns(Task.FromResult<string?>(ExpectedLongUrl));

        mock.Setup(ls => ls.GetLongUrlByToken(It.IsAny<string>()))
            .Returns(Task.FromResult<string?>(ExpectedLongUrl));

        return mock;
    }

}

