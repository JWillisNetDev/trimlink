
using AutoMapper;
using Moq;
using trimlink.core.Services;
using trimlink.tests.Mocks;
using trimlink.website.Configuration;
using trimlink.website.Controllers;

namespace trimlink.tests.UnitTests;

[TestFixture]
public class LinksControllerTests
{
    IMapper mapper;
    Mock<ILinkService> mockLinkService;
    LinksController controller;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        MapperConfiguration config = new(cfg => cfg.AddProfile<MappingProfile>());
        mapper = new Mapper(config);

        mockLinkService = MockLinkService.GetMock();

        controller = new LinksController(mapper, mockLinkService.Object);
    }

    [Test]
    public void LinksController_PostNewLink_ReturnsCreatedLinkDetails()
    {
        

    }
}
