
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using trimlink.core.Services;
using trimlink.tests.Mocks;
using trimlink.website.Configuration;
using trimlink.website.Contracts;
using trimlink.website.Controllers;

namespace trimlink.tests.UnitTests.website;

[TestFixture]
public class LinksControllerTests
{
    // Disables no-default-values warnings.
    #pragma warning disable CS8618
    
    private IMapper _mapper;
    private Mock<ILogger<LinksController>> _mockLogger;
    private Mock<ILinkService> _mockLinkService;
    private Mock<IUrlHelper> _mockUrlHelper;
    private LinksController _controller;
    
    #pragma warning restore CS8618
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        MapperConfiguration config = new(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = new Mapper(config);

        _mockLogger = new Mock<ILogger<LinksController>>();
        _mockLinkService = MockLinkService.GetMock();
        _mockUrlHelper = MockUrlHelper.GetMock();

        _controller = new LinksController(_mockLogger.Object, _mapper, _mockLinkService.Object);
        _controller.Url = _mockUrlHelper.Object;
    }

    [Test]
    public async Task LinksController_PostNewLink_ReturnsCreatedShortId()
    {
        LinkCreateDto linkCreate = new()
        {
            RedirectToUrl = "https://www.google.com/",
            Duration = TimeSpan.FromHours(42)
        };

        ObjectResult? actual = await _controller.CreateLink(linkCreate) as ObjectResult;

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.AssignableFrom<CreatedResult>());
        Assert.That(actual?.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(actual?.Value, Is.Not.Null);
    }
}