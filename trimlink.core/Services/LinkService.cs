using System.Diagnostics.CodeAnalysis;
using trimlink.data;
using trimlink.data.Models;
using trimlink.data.Repositories;
using shortid;
using Microsoft.EntityFrameworkCore;

namespace trimlink.core.Services;

public class LinkService : ILinkService
{
    private static Link CreateLink(string toUrl, bool isNeverExpires, TimeSpan expiresAfter)
    {
        string shortId = ShortId.Generate();
        DateTime now = DateTime.UtcNow;
        Link link = new Link()
        {
            RedirectToUrl = toUrl,
            ShortId = shortId,
            IsMarkedForDeletion = false,
            UtcDateCreated = now,
            IsNeverExpires = isNeverExpires,
            UtcDateExpires = isNeverExpires ? DateTime.MaxValue : now + expiresAfter,
        };
        return link;
    }

    private static Link CreateLink(string toUrl)
        => CreateLink(toUrl, true, TimeSpan.Zero);

    public LinkService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    private IUnitOfWork CreateUnitOfWork() => _unitOfWorkFactory.CreateUnitOfWork();

    public string GenerateShortLink(string toUrl, out int id)
    {
        ArgumentNullException.ThrowIfNull(toUrl, nameof(toUrl));
        using IUnitOfWork unitOfWork = CreateUnitOfWork();

        Link link = CreateLink(toUrl, true, TimeSpan.Zero);

        unitOfWork.Links.Add(link);
        unitOfWork.Save();

        id = link.Id;
        return link.ShortId;
    }

    public string GenerateShortLink(string toUrl, TimeSpan expiresAfter, out int id)
    {
        ArgumentNullException.ThrowIfNull(toUrl, nameof(toUrl));
        using IUnitOfWork unitOfWork = CreateUnitOfWork();

        Link link = CreateLink(toUrl, false, expiresAfter);

        unitOfWork.Links.Add(link);
        unitOfWork.Save();

        id = link.Id;
        return link.ShortId;
    }

    public string? GetLongUrlById(int id)
    {
        using IUnitOfWork unitOfWork = CreateUnitOfWork();

        Link? found = unitOfWork.Links.Get(id);
        return found?.RedirectToUrl;
    }

    public string? GetLongUrlByShortId(string shortId)
    {
        using IUnitOfWork unitOfWork = CreateUnitOfWork();

        Link? found = unitOfWork.Links.Find(link => link.ShortId == shortId);
        return found?.RedirectToUrl;
    }
}