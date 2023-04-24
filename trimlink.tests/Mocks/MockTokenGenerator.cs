using Moq;
using trimlink.core;

namespace trimlink.tests.Mocks;

internal class MockTokenGenerator
{
    public static Mock<ITokenGenerator> GetMock()
    {
        Mock<ITokenGenerator> mock = new();

        mock.Setup(gen => gen.GenerateToken())
            .Returns("QYpEGR2fqg2z");

        return mock;
    }
}

