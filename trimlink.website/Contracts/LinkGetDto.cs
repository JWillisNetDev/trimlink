namespace trimlink.website.Contracts;

public class LinkGetDto
{
    public int Id { get; set; }
    public DateTime UtcDateCreated { get; set; }
    public DateTime UtcDateExpires { get; set; }
    public bool IsNeverExpires { get; set; }

    public string Token { get; set; } = string.Empty;
    public string RedirectToUrl { get; set; } = string.Empty;
}