namespace trimlink.core.Services;

public interface ILinkService
{
    Task<string> GenerateShortLink(string toUrl, TimeSpan? expiresAfter = null);

    Task<string?> GetLongUrlById(int id);
    Task<string?> GetLongUrlByToken(string token);

    Task<Records.LinkDetails?> GetLinkDetailsById(int id);
    Task<Records.LinkDetails?> GetLinkDetailsByToken(string token);
}