using System.Text.Json.Serialization;

namespace trimlink.website.Contracts;

public class LinkCreateDto
{
    public string RedirectToUrl { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public bool IsNeverExpires { get; set; } = false;
}