using Moq;
using trimlink.core;
using trimlink.core.Records;
using trimlink.core.Services;
using trimlink.data;
using trimlink.data.Models;
using trimlink.tests.Mocks;
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable StringLiteralTypo

namespace trimlink.tests.UnitTests.core;

[TestFixture]
public class LinkServiceTests
{
    /// <summary>Do not directly access this field <i>(unless if you know what you're doing)</i>.<br />Use <see cref="LinkService"/> instead.</summary>
    private LinkService? _linkService;
    private LinkService LinkService => _linkService ??= new LinkService(_mockContext.Object, _tokenGenerator, _linkValidator);
    
    // Disables no-default-values warnings
    #pragma warning disable CS8618
    
    Mock<TrimLinkDbContext> _mockContext;
    ITokenGenerator _tokenGenerator;
    ILinkValidator _linkValidator;
    
    #pragma warning restore CS8618
    
    [SetUp]
    public void SetUp()
    {
        List<Link> linksData = new()
        {
            new Link()
            {
                Id = 0,
                UtcDateCreated = DateTime.Parse("2023-03-26 12:00:00.000"),
                UtcDateExpires = DateTime.Parse("2024-03-26 12:00:00.000"),
                IsNeverExpires = false,
                IsMarkedForDeletion = false,
                Token = "UZuMieEQHEha",
                RedirectToUrl = @"https://www.google.com/"
            },
            new Link()
            {
                Id = 1,
                UtcDateCreated = DateTime.Parse("2023-10-10 12:00:00.000"),
                UtcDateExpires = DateTime.MaxValue,
                IsNeverExpires = true,
                IsMarkedForDeletion = false,
                Token = "sVFwysRodYWB",
                RedirectToUrl = @"https://www.youtube.com/"
            }
        };

        _mockContext = MockTrimLinkDbContext.GetMock(linksData);
        _tokenGenerator = new TestTokenGenerator();
        _linkValidator = new TestLinkValidator();
    }

    [TearDown]
    public void TearDown()
    {
        _linkService?.Dispose();
        _linkService = null;
    }

    [Test]
    public void GenerateShortLink_GivenNoExpiration_ReturnsToken()
    {
        const string expectedToken = TestTokenGenerator.ExpectedToken;
        const string toUrl = "https://www.google.com/";

        string actualToken = LinkService.GenerateShortLink(toUrl, out int actualId);

        Assert.Multiple(() =>
        {
            Assert.That(actualId, Is.AtLeast(0));
            Assert.That(actualToken, Is.Not.Null.Or.Empty);
            Assert.That(actualToken, Is.EqualTo(expectedToken));
        });
    }

    [Test]
    public void GenerateShortLink_GivenExpiration_ReturnsToken()
    {
        const string expectedToken = TestTokenGenerator.ExpectedToken;
        const string toUrl = "https://www.google.com/";
        TimeSpan expiresAfter = TimeSpan.FromDays(1);

        string actualToken = LinkService.GenerateShortLink(toUrl, expiresAfter, out int actualId);
        
        Assert.Multiple(() => {
            Assert.That(actualId, Is.AtLeast(0));
            Assert.That(actualToken, Is.Not.Null.Or.Empty);
            Assert.That(actualToken, Is.EqualTo(expectedToken));
        });
    }

    [Test]
    public void GenerateShortLink_GivenInvalidLink_ThrowsArgumentException()
    {
        const int expectedId = -1;
        const string url = "some malformed url";
        int actualId = expectedId;

        Assert.Throws<ArgumentException>(() =>
        {
            LinkService.GenerateShortLink(url, out actualId);
        });
        Assert.That(actualId, Is.EqualTo(expectedId));
    }

    [Test]
    public void GenerateShortLink_GivenNullUrl_ThrowsArgumentNullException()
    {
        const int expectedId = -1;
        const string url = null!;
        int actualId = expectedId;

        Assert.Throws<ArgumentNullException>(() =>
        {
            LinkService.GenerateShortLink(url!, out actualId);
        });
        Assert.That(actualId, Is.EqualTo(expectedId));
    }

    [Test]
    public void GenerateShortLink_GivenEmptyUrl_ThrowsArgumentException()
    {
        const int expectedId = -1;
        const string url = "";
        int actualId = expectedId;

        Assert.Throws<ArgumentException>(() =>
        {
            LinkService.GenerateShortLink(url, out actualId);
        });
        Assert.That(actualId, Is.EqualTo(expectedId));
    }

    [Test]
    public void GenerateShortLink_GivenInvalidRelative_ThrowsLinkValidationException()
    {
        const string url = "/this/is/relative";

        Assert.Throws<ArgumentException>(() =>
        {
            LinkService.GenerateShortLink(url, out int _);
        });
    }

    [Test]
    public void GetLongUrlById_GivenValidId_ReturnsLongUrl()
    {
        const int id = 1;
        const string expectedUrl = "https://www.youtube.com/";

        string? actualUrl = LinkService.GetLongUrlById(id);

        Assert.That(actualUrl, Is.Not.Null.Or.Empty);
        Assert.That(actualUrl, Is.EqualTo(expectedUrl));
    }

    [Test]
    public void GetLongUrlById_GivenInvalidId_ReturnsNull()
    {
        const int id = 42_1337;

        string? actualUrl = LinkService.GetLongUrlById(id);

        Assert.That(actualUrl, Is.Null);
    }

    [Test]
    public void GetLongUrlByToken_GivenValidToken_ReturnsLongUrl()
    {
        const string token = "UZuMieEQHEha";
        const string expectedUrl = "https://www.google.com/";

        string? actualUrl = LinkService.GetLongUrlByToken(token);

        Assert.That(actualUrl, Is.Not.Null.Or.Empty);
        Assert.That(actualUrl, Is.EqualTo(expectedUrl));
    }

    [Test]
    public void GetLongUrlByToken_GivenInvalidToken_ReturnsNull()
    {
        const string token = "hello, world!";

        string? actualUrl = LinkService.GetLongUrlByToken(token);

        Assert.That(actualUrl, Is.Null);
    }

    [Test]
    public void GetLinkDetailsById_GivenValidId_ReturnsLinkDetails()
    {
        const int expectedId = 0;

        LinkDetails? actual = LinkService.GetLinkDetailsById(expectedId);

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual?.Id, Is.EqualTo(expectedId));
    }

    [Test]
    public void GetLinkDetailsById_GivenInvalidId_ReturnsNull()
    {
        const int expectedId = 42;

        LinkDetails? actual = LinkService.GetLinkDetailsById(expectedId);

        Assert.That(actual, Is.Null);
    }

    [Test]
    public void GetLinkDetailsByToken_GivenValidToken_ReturnsLinkDetails()
    {
        const string expectedToken = "UZuMieEQHEha";

        LinkDetails? actual = LinkService.GetLinkDetailsByToken(expectedToken);

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual?.Token, Is.EqualTo(expectedToken));
    }

    [Test]
    public void GetLinkDetailsByToken_GivenInvalidToken_ReturnsNull()
    {
        const string expectedToken = "inigo montoya";

        LinkDetails? actual = LinkService.GetLinkDetailsByToken(expectedToken);

        Assert.That(actual, Is.Null);
    }
}