using System.Text.Json.Serialization;

namespace trimlink.website.Contracts;

public class LinkCreateDto
{
    public string RedirectToUrl { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public bool IsNeverExpires { get; set; }
}