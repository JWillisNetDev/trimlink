using System.Diagnostics.CodeAnalysis;
using trimlink.data;
using trimlink.data.Models;
using trimlink.data.Repositories;
using shortid;
using Microsoft.EntityFrameworkCore;
using trimlink.core.Records;

namespace trimlink.core.Services;

public class LinkService : ILinkService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly ILinkValidator _linkValidator;

    public LinkService(
        IUnitOfWorkFactory unitOfWorkFactory,
        ITokenGenerator tokenGenerator,
        ILinkValidator linkValidator
    )
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _tokenGenerator = tokenGenerator;
        _linkValidator = linkValidator;
    }

    public LinkService(IUnitOfWorkFactory unitOfWorkFactory, ITokenGenerator tokenGenerator) : this(unitOfWorkFactory, tokenGenerator, new DefaultLinkValidator())
    {
    }

    public LinkService(IUnitOfWorkFactory unitOfWorkFactory, ILinkValidator linkValidator) : this(unitOfWorkFactory, new DefaultTokenGenerator(), linkValidator)
    {
    }

    public LinkService(IUnitOfWorkFactory unitOfWorkFactory) : this(unitOfWorkFactory, new DefaultTokenGenerator(), new DefaultLinkValidator())
    {
    }

    private Link CreateLink(string toUrl, bool isNeverExpires, TimeSpan expiresAfter)
    {
        string token = _tokenGenerator.GenerateToken();
        DateTime now = DateTime.UtcNow;
        Link link = new()
        {
            RedirectToUrl = toUrl,
            Token = token,
            IsMarkedForDeletion = false,
            IsNeverExpires = isNeverExpires,
            UtcDateCreated = now,
            UtcDateExpires = isNeverExpires ? DateTime.MaxValue : now + expiresAfter,
        };
        return link;
    }

    private Link CreateLink(string toUrl)
        => CreateLink(toUrl, true, TimeSpan.Zero);

    private IUnitOfWork CreateUnitOfWork()
        => _unitOfWorkFactory.CreateUnitOfWork();

    private void HandleLinkValidation(string toUrl)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(toUrl);
        LinkValidationResult result = _linkValidator.Validate(toUrl);
        if (result == LinkValidationResult.InvalidScheme)
        {
            throw new ArgumentException($"The given URL ({toUrl}) has an invalid scheme.");
        }
        else if (result == LinkValidationResult.InvalidRelative)
        {
            throw new ArgumentException($"The given URL ({toUrl}) cannot be a relative path.");
        }
    }

    public string GenerateShortLink(string toUrl, out int id)
    {
        HandleLinkValidation(toUrl);

        Link link = CreateLink(toUrl, true, TimeSpan.Zero);

        using IUnitOfWork unitOfWork = CreateUnitOfWork();
        unitOfWork.Links.Add(link);
        unitOfWork.Save();

        id = link.Id;
        return link.Token;
    }

    public string GenerateShortLink(string toUrl, TimeSpan expiresAfter, out int id)
    {
        HandleLinkValidation(toUrl);

        Link link = CreateLink(toUrl, false, expiresAfter);

        using IUnitOfWork unitOfWork = CreateUnitOfWork();
        unitOfWork.Links.Add(link);
        unitOfWork.Save();

        id = link.Id;
        return link.Token;
    }

    public string? GetLongUrlById(int id)
    {
        using IUnitOfWork unitOfWork = CreateUnitOfWork();
        Link? found = unitOfWork.Links.Get(id);
        return found?.RedirectToUrl;
    }

    public string? GetLongUrlByToken(string shortId)
    {
        using IUnitOfWork unitOfWork = CreateUnitOfWork();
        Link? found = unitOfWork.Links.Find(link => link.Token == shortId);
        return found?.RedirectToUrl;
    }

    public LinkDetails? GetLinkDetailsById(int id)
    {
        using IUnitOfWork unitOfWork = CreateUnitOfWork();
        Link? found = unitOfWork.Links.Get(id);
        return found is null ?
            null :
            new LinkDetails(found);
    }

    public LinkDetails? GetLinkDetailsByToken(string token)
    {
        using IUnitOfWork unitOfWork = CreateUnitOfWork();
        Link? found = unitOfWork.Links.Find(link => link.Token == token);
        return found is null ?
            null :
            new LinkDetails(found);
    }
}