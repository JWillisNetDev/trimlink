namespace trimlink.core.Services;

public class LinkValidationException : Exception
{
    public LinkValidationResult ValidationResult { get; }
    
    public LinkValidationException(LinkValidationResult result, string? message = null, Exception? innerException = null) : base(message, innerException)
    {
        ValidationResult = result;
    }
}