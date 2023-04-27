using trimlink.data.Models;

namespace trimlink.core.Records;

public record class LinkDetails(int Id, string RedirectToUrl, string Token, bool Expires, DateTime UtcDateCreated, DateTime UtcDateExpires)
{
    public int Id { get; } = Id;
    public string RedirectToUrl { get; } = RedirectToUrl ?? throw new ArgumentNullException(nameof(RedirectToUrl));
    public string Token { get; } = Token ?? throw new ArgumentNullException(nameof(Token));
    public bool IsNeverExpires { get; } = Expires;
    public DateTime UtcDateCreated { get; } = UtcDateCreated;
    public DateTime UtcDateExpires { get; } = UtcDateExpires;

    internal LinkDetails(Link link) : this(link.Id, link.RedirectToUrl, link.Token, link.IsNeverExpires, link.UtcDateCreated, link.UtcDateExpires)
    {
    }
}