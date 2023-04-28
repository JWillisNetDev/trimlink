using trimlink.data;
using trimlink.data.Models;
using Microsoft.EntityFrameworkCore;
using trimlink.core.Records;

namespace trimlink.core.Services;

public class LinkService : ILinkService, IDisposable
{
    private readonly TrimLinkDbContext _context;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly ILinkValidator _linkValidator;

    public LinkService(
        TrimLinkDbContext context,
        ITokenGenerator tokenGenerator,
        ILinkValidator linkValidator
    )
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
        _linkValidator = linkValidator;
    }

    public LinkService(TrimLinkDbContext context, ITokenGenerator tokenGenerator) : this(context, tokenGenerator, new DefaultLinkValidator())
    {
    }

    public LinkService(TrimLinkDbContext context, ILinkValidator linkValidator) : this(context, new DefaultTokenGenerator(), linkValidator)
    {
    }

    public LinkService(TrimLinkDbContext context) : this(context, new DefaultTokenGenerator(), new DefaultLinkValidator())
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
        ArgumentException.ThrowIfNullOrEmpty(toUrl);
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

        Link link = CreateLink(toUrl);

        _context.Links.Add(link);
        _context.SaveChanges();

        id = link.Id;
        return link.Token;
    }

    public string GenerateShortLink(string toUrl, TimeSpan expiresAfter, out int id)
    {
        HandleLinkValidation(toUrl);

        Link link = CreateLink(toUrl, false, expiresAfter);

        _context.Links.Add(link);
        _context.SaveChanges();

        id = link.Id;
        return link.Token;
    }

    public string? GetLongUrlById(int id)
    {
        Link? found = _context.Links
            .AsNoTracking()
            .SingleOrDefault(link => link.Id == id);

        return found?.RedirectToUrl;
    }

    public string? GetLongUrlByToken(string token)
    {
        Link? found = _context.Links
            .AsNoTracking()
            .SingleOrDefault(link => link.Token == token);

        return found?.RedirectToUrl;
    }

    public LinkDetails? GetLinkDetailsById(int id)
    {
        Link? found = _context.Links
            .AsNoTracking()
            .SingleOrDefault(link => link.Id == id);

        return found is null ?
            null :
            new LinkDetails(found);
    }

    public LinkDetails? GetLinkDetailsByToken(string token)
    {
        Link? found = _context.Links
            .AsNoTracking()
            .SingleOrDefault(link => link.Token == token);

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
            _context.Dispose();
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