using Moq;
using trimlink.core;
using trimlink.core.Interfaces;
using trimlink.core.Records;
using trimlink.core.Services;
using trimlink.data;
using trimlink.data.Models;
using trimlink.tests.Doubles;
using trimlink.tests.Mocks;
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable StringLiteralTypo

namespace trimlink.tests.UnitTests.core;

[TestFixture]
public class LinkServiceTests
{
    #pragma warning disable CS8618 // No default values

    private IUnitOfWork _unitOfWork;
    private IDateTimeProvider _dateTimeProvider;
    private ITokenGenerator _tokenGenerator;
    private ILinkValidator _linkValidator;
    private ILinkService _linkService;
    
    #pragma warning restore CS8618 // No default values
    
    [SetUp]
    public void SetUp()
    {
        List<Link> linkData = new()
        {
            new Link
            {
                Id = 0,
                UtcDateCreated = DateTime.Parse("2023-03-26 12:00:00.000"),
                UtcDateExpires = DateTime.Parse("2024-03-26 12:00:00.000"),
                IsMarkedForDeletion = false,
                Token = "UZuMieEQHEha",
                RedirectToUrl = @"https://www.google.com/"
            },
            new Link
            {
                Id = 1,
                UtcDateCreated = DateTime.Parse("2023-10-10 12:00:00.000"),
                UtcDateExpires = null,
                IsMarkedForDeletion = false,
                Token = "sVFwysRodYWB",
                RedirectToUrl = @"https://www.youtube.com/"
            }
        };

        _unitOfWork = Mock.Of<IUnitOfWork>();
        Mock.Get(_unitOfWork)
            .Setup(work => work.Links)
            .Returns(new TestLinkRepository(linkData));
        Mock.Get(_unitOfWork)
            .Setup(work => work.Save())
            .Verifiable();
        
        _dateTimeProvider = Mock.Of<IDateTimeProvider>();
        Mock.Get(_dateTimeProvider)
            .Setup(dt => dt.Now)
            .Returns(new DateTime(2022, 4, 1, 12, 30, 30));
        Mock.Get(_dateTimeProvider)
            .Setup(dt => dt.UtcNow)
            .Returns(new DateTime(2023, 4, 1, 20, 30, 30));
        Mock.Get(_dateTimeProvider)
            .Setup(dt => dt.TimeZone)
            .Returns(TimeZoneInfo.Utc);

        _tokenGenerator = Mock.Of<ITokenGenerator>();
        Mock.Get(_tokenGenerator)
            .Setup(t => t.GenerateToken())
            .Returns("2AVncedzFU2h");

        _linkValidator = Mock.Of<ILinkValidator>();
        Mock.Get(_linkValidator)
            .Setup(t => t.Validate(It.IsAny<string>()))
            .Returns(LinkValidationResult.Valid);

        _linkService = new LinkService(_unitOfWork, _dateTimeProvider, _tokenGenerator, _linkValidator);
    }

    [Test]
    public async Task GenerateShortLink_GivenNoExpiration_ReturnsToken()
    {
        const string expectedToken = "2AVncedzFU2h";
        const string toUrl = "https://www.google.com/";

        string actualToken = await _linkService.GenerateShortLink(toUrl);

        Assert.Multiple(() =>
        {
            Assert.That(actualToken, Is.Not.Null.Or.Empty);
            Assert.That(actualToken, Is.EqualTo(expectedToken));
            Mock.Get(_unitOfWork).Verify(work => work.Save());
        });
    }

    [Test]
    public async Task GenerateShortLink_GivenExpiration_ReturnsToken()
    {
        const string expectedToken = "2AVncedzFU2h";
        const string toUrl = "https://www.google.com/";
        TimeSpan expiresAfter = TimeSpan.FromDays(1);

        string actualToken = await _linkService.GenerateShortLink(toUrl, expiresAfter);
        
        Assert.Multiple(() => {
            Assert.That(actualToken, Is.Not.Null.Or.Empty);
            Assert.That(actualToken, Is.EqualTo(expectedToken));
        });
    }

    [Test]
    public void GenerateShortLink_GivenInvalidLink_ThrowsArgumentException()
    {
        Mock.Get(_linkValidator)
            .Setup(t => t.Validate(It.IsAny<string>()))
            .Returns(LinkValidationResult.InvalidRelative);
        
        const string url = "some malformed url";

        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _linkService.GenerateShortLink(url);
        });
    }

    [Test]
    public void GenerateShortLink_GivenNullUrl_ThrowsArgumentNullException()
    {
        const string url = null!;

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _linkService.GenerateShortLink(url!);
        });
    }

    [Test]
    public void GenerateShortLink_GivenEmptyUrl_ThrowsArgumentException()
    {
        const string url = "";

        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _linkService.GenerateShortLink(url);
        });
    }

    [Test]
    public async Task GetLongUrlById_GivenValidId_ReturnsLongUrl()
    {
        const int id = 1;
        const string expectedUrl = "https://www.youtube.com/";

        string? actualUrl = await _linkService.GetLongUrlById(id);

        Assert.That(actualUrl, Is.Not.Null.Or.Empty);
        Assert.That(actualUrl, Is.EqualTo(expectedUrl));
    }

    [Test]
    public async Task GetLongUrlById_GivenInvalidId_ReturnsNull()
    {
        const int id = 42_1337;

        string? actualUrl = await _linkService.GetLongUrlById(id);

        Assert.That(actualUrl, Is.Null);
    }

    [Test]
    public async Task GetLongUrlByToken_GivenValidToken_ReturnsLongUrl()
    {
        const string token = "UZuMieEQHEha";
        const string expectedUrl = "https://www.google.com/";

        string? actualUrl = await _linkService.GetLongUrlByToken(token);

        Assert.That(actualUrl, Is.Not.Null.Or.Empty);
        Assert.That(actualUrl, Is.EqualTo(expectedUrl));
    }

    [Test]
    public async Task GetLongUrlByToken_GivenInvalidToken_ReturnsNull()
    {
        const string token = "hello, world!";

        string? actualUrl = await _linkService.GetLongUrlByToken(token);

        Assert.That(actualUrl, Is.Null);
    }

    [Test]
    public async Task GetLinkDetailsById_GivenValidId_ReturnsLinkDetails()
    {
        const int expectedId = 0;

        LinkDetails? actual = await _linkService.GetLinkDetailsById(expectedId);

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual?.Id, Is.EqualTo(expectedId));
    }

    [Test]
    public async Task GetLinkDetailsById_GivenInvalidId_ReturnsNull()
    {
        const int expectedId = 42;

        LinkDetails? actual = await _linkService.GetLinkDetailsById(expectedId);

        Assert.That(actual, Is.Null);
    }

    [Test]
    public async Task GetLinkDetailsByToken_GivenValidToken_ReturnsLinkDetails()
    {
        const string expectedToken = "UZuMieEQHEha";

        LinkDetails? actual = await _linkService.GetLinkDetailsByToken(expectedToken);

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual?.Token, Is.EqualTo(expectedToken));
    }

    [Test]
    public async Task GetLinkDetailsByToken_GivenInvalidToken_ReturnsNull()
    {
        const string expectedToken = "inigo montoya";

        LinkDetails? actual = await _linkService.GetLinkDetailsByToken(expectedToken);

        Assert.That(actual, Is.Null);
    }
}