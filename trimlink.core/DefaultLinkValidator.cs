namespace trimlink.core;

public class DefaultLinkValidator : ILinkValidator
{
    public DefaultLinkValidator()
    {
    }

    public LinkValidationResult Validate(string url)
    {
        return LinkValidationResult.Valid;
    }
}

