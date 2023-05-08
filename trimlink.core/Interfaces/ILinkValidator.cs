namespace trimlink.core;

public interface ILinkValidator
{
    LinkValidationResult Validate(string url);
}

