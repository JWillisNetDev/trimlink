using trimlink.core.Interfaces;

namespace trimlink.core;

public class LinkValidator : ILinkValidator
{
    public LinkValidationResult Validate(string url)
    {
        return LinkValidationResult.Valid;
    }
}