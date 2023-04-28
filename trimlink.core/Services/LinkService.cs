using System.Diagnostics.CodeAnalysis;
using trimlink.data;
using trimlink.data.Models;
using trimlink.data.Repositories;
using shortid;
using Microsoft.EntityFrameworkCore;
using trimlink.core.Records;

namespace trimlink.core.Services;

public class LinkService : ILinkService, IDisposable
{
    //private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly ILinkValidator _linkValidator;

    public LinkService(
        IUnitOfWorkFactory unitOfWorkFactory,
        ITokenGenerator tokenGenerator,
        ILinkValidator linkValidator
    )
    {
        _unitOfWork = new Lazy<IUnitOfWork>(() => unitOfWorkFactory.CreateUnitOfWork(), true);
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

        _unitOfWork.Value.Links.Add(link);
        _unitOfWork.Value.Save();

        id = link.Id;
        return link.Token;
    }

    public string GenerateShortLink(string toUrl, TimeSpan expiresAfter, out int id)
    {
        HandleLinkValidation(toUrl);

        Link link = CreateLink(toUrl, false, expiresAfter);

        _unitOfWork.Value.Links.Add(link);
        _unitOfWork.Value.Save();

        id = link.Id;
        return link.Token;
    }

    public string? GetLongUrlById(int id)
    {
        Link? found = _unitOfWork.Value.Links.Get(id);
        return found?.RedirectToUrl;
    }

    public string? GetLongUrlByToken(string shortId)
    {
        Link? found = _unitOfWork.Value.Links.Find(link => link.Token == shortId);
        return found?.RedirectToUrl;
    }

    public LinkDetails? GetLinkDetailsById(int id)
    {
        Link? found = _unitOfWork.Value.Links.Get(id);
        return found is null ?
            null :
            new LinkDetails(found);
    }

    public LinkDetails? GetLinkDetailsByToken(string token)
    {
        Link? found = _unitOfWork.Value.Links.Find(link => link.Token == token);
        return found is null ?
            null :
            new LinkDetails(found);
    }

    #region Implements IDisposable
    private bool _disposed;
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            if (_unitOfWork.IsValueCreated &&
                _unitOfWork.Value is IDisposable disposable)
                disposable.Dispose();

            _disposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}