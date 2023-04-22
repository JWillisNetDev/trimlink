using System.Diagnostics.CodeAnalysis;
using trimlink.data;
using trimlink.data.Models;
using trimlink.data.Repositories;
using shortid;

namespace trimlink.core.Services;

public class LinkService : ILinkService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    [SetsRequiredMembers]
    public LinkService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public string GenerateShortLink(string toUrl, out int id)
    {
        ArgumentNullException.ThrowIfNull(toUrl, nameof(toUrl));
        using UnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();

        Link link = new Link
        {
            RedirectToUrl = toUrl,
            UtcDateCreated = DateTime.UtcNow,
            UtcDateExpires = DateTime.MaxValue,
            IsNeverExpires = true,
            ShortId = ShortId.Generate(),
        };

        unitOfWork.Links.Add(link);
        unitOfWork.Save();

        id = link.Id;
        return link.ShortId;
    }

    public string GenerateShortLink(string toUrl, TimeSpan expiresAfter, out int id)
    {
        ArgumentNullException.ThrowIfNull(toUrl, nameof(toUrl));
        using UnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();

        DateTime now = DateTime.UtcNow;
        Link link = new Link
        {
            RedirectToUrl = toUrl,
            UtcDateCreated = now,
            UtcDateExpires = now + expiresAfter,
            IsNeverExpires = false,
            ShortId = ShortId.Generate(),
        };

        unitOfWork.Links.Add(link);
        unitOfWork.Save();

        id = link.Id;
        return link.ShortId;
    }

    public string? GetLongUrlById(int id)
    {
        using UnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();

        Link? found = unitOfWork.Links.GetById(id);
        return found?.RedirectToUrl;
    }

    public string? GetLongUrlByShortId(string shortId)
    {
        using UnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();

        Link? found = unitOfWork.Links.Find(link => link.ShortId == shortId);
        return found?.RedirectToUrl;
    }
}