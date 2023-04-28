namespace trimlink.core;

public class DefaultLinkValidator : ILinkValidator
{
    public LinkValidationResult Validate(string url)
    {
        return LinkValidationResult.Valid;
    }
}