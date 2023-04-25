
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
    IMapper mapper;
    Mock<ILogger<LinksController>> mockLogger;
    Mock<ILinkService> mockLinkService;
    Mock<IUrlHelper> mockUrlHelper;
    LinksController controller;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        MapperConfiguration config = new(cfg => cfg.AddProfile<MappingProfile>());
        mapper = new Mapper(config);

        mockLogger = new Mock<ILogger<LinksController>>();
        mockLinkService = MockLinkService.GetMock();
        mockUrlHelper = MockUrlHelper.GetMock();

        controller = new LinksController(mockLogger.Object, mapper, mockLinkService.Object);
        controller.Url = mockUrlHelper.Object;
    }

    [Test]
    public void LinksController_PostNewLink_ReturnsCreatedShortId()
    {
        LinkCreateDto linkCreate = new()
        {
            RedirectToUrl = "https://www.google.com/",
            Duration = "7.00:00",
            IsNeverExpires = false,
        };

        ObjectResult? actual = controller.CreateLink(linkCreate) as ObjectResult;

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.AssignableFrom<CreatedResult>());
        Assert.That(actual?.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(actual?.Value, Is.Not.Null);
    }
}
