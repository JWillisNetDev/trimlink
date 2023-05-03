using System.ComponentModel.DataAnnotations;

namespace trimlink.website.Contracts;

public class LinkCreateDto
{
    [Required, Url]
    public string RedirectToUrl { get; init; } = string.Empty;
    public TimeSpan? Duration { get; init; }
}