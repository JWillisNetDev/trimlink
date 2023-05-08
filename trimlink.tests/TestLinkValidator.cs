using trimlink.core;
using trimlink.core.Interfaces;

namespace trimlink.tests;

internal class TestLinkValidator : ILinkValidator
{
    public LinkValidationResult Validate(string url)
    {
        if (url.StartsWith('/'))
            return LinkValidationResult.InvalidRelative;

        else if (!url.ToLower().StartsWith("https"))
            return LinkValidationResult.InvalidScheme;

        return LinkValidationResult.Valid;
    }
}

