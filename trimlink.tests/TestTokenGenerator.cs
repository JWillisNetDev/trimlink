using trimlink.core;
using trimlink.core.Interfaces;

namespace trimlink.tests;

internal class TestTokenGenerator : ITokenGenerator
{
    public const string ExpectedToken = "2AVncedzFU2h";

    public string GenerateToken()
    {
        return ExpectedToken;
    }
}