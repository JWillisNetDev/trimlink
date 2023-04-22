using System.Diagnostics.CodeAnalysis;

namespace trimlink.core.Services;

public interface ILinkService
{
    string GenerateShortLink(string toUrl, out int id);
    string GenerateShortLink(string toUrl, TimeSpan expiresAfter, out int id);

    string? GetLongUrlById(int id);
    string? GetLongUrlByShortId(string shortId);
}