using trimlink.data;
using trimlink.data.Models;
using Microsoft.EntityFrameworkCore;
using trimlink.core.Records;

namespace trimlink.core.Services;

public class LinkService : ILinkService, IAsyncDisposable, IDisposable
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

    private Link CreateLink(string toUrl, TimeSpan? expiresAfter = null)
    {
        string token = _tokenGenerator.GenerateToken();
        DateTime now = DateTime.UtcNow;
        Link link = new()
        {
            RedirectToUrl = toUrl,
            Token = token,
            IsMarkedForDeletion = false,
            UtcDateCreated = now,
            UtcDateExpires = now + expiresAfter // This evaluates to 'null' if either side of expression is null! Nifty.
        };
        return link;
    }

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

    public async Task<string> GenerateShortLink(string toUrl, TimeSpan? expiresAfter = null)
    {
        HandleLinkValidation(toUrl);

        Link link = CreateLink(toUrl, expiresAfter);
        await _context.Links.AddAsync(link);
        await _context.SaveChangesAsync();

        return link.Token;
    }

    public async Task<string?> GetLongUrlById(int id)
    {
        Link? found = await _context.Links
            .AsNoTracking()
            .SingleOrDefaultAsync(link => link.Id == id);
        return found?.RedirectToUrl;
    }

    public async Task<string?> GetLongUrlByToken(string token)
    {
        Link? found = await _context.Links
            .AsNoTracking()
            .SingleOrDefaultAsync(link => link.Token == token);
        return found?.RedirectToUrl;
    }

    public async Task<LinkDetails?> GetLinkDetailsById(int id)
    {
        Link? found = await _context.Links
            .AsNoTracking()
            .SingleOrDefaultAsync(link => link.Id == id);

        return found is null ?
            null :
            new LinkDetails(found);
    }

    public async Task<LinkDetails?> GetLinkDetailsByToken(string token)
    {
        Link? found = await _context.Links
            .AsNoTracking()
            .SingleOrDefaultAsync(link => link.Token == token);
        return found is null ?
            null :
            new LinkDetails(found);
    }
    
    #region Implements IAsyncDisposable
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        
        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        await _context.DisposeAsync().ConfigureAwait(false);
    }
    
    #endregion
    #region Implements IDisposable
    private bool _disposed;
    
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    
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
    #endregion
}