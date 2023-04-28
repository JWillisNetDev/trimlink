namespace trimlink.website.Contracts;

public class LinkCreateDto
{
    public string RedirectToUrl { get; init; } = string.Empty;
    public TimeSpan Duration { get; init; } = TimeSpan.Zero;
    public bool IsNeverExpires { get; init; }
}