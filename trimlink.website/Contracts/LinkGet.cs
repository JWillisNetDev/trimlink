namespace trimlink.website.Contracts;

public class LinkGet
{
    public int Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateExpires { get; set; }
    public bool IsNeverExpires { get; set; }

    public string ShortId { get; set; } = string.Empty;
    public string RedirectToUrl { get; set; } = string.Empty;
    public string TrimmedUrl { get; set; } = string.Empty;
}