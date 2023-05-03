namespace trimlink.core.Services;

public interface ILinkService
{
    Task<string> GenerateShortLink(string toUrl, TimeSpan? expiresAfter = null);

    string? GetLongUrlById(int id);
    string? GetLongUrlByToken(string token);

    Records.LinkDetails? GetLinkDetailsById(int id);
    Records.LinkDetails? GetLinkDetailsByToken(string token);
}