using Moq;
using trimlink.core;

namespace trimlink.tests.Mocks;

internal class MockLinkValidator
{
    public static Mock<ILinkValidator> GetMock()
    {
        Mock<ILinkValidator> mock = new();

        mock.Setup(validator => validator.Validate(It.IsAny<string>()))
            .Returns(LinkValidationResult.Valid);

        return mock;
    }
}

