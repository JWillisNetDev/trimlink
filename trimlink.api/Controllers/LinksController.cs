using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trimlink.data;
using trimlink.data.Models;
using trimlink.api.Contracts;
using AutoMapper;

namespace trimlink.api.Controllers;

[ApiController, Route("api/[controller]")]
public class LinksController : Controller
{
    protected IMapper Mapper { get; }
    protected IDbContextFactory<TrimLinkDbContext> DbContextFactory { get; }

    public LinksController(IMapper mapper, IDbContextFactory<TrimLinkDbContext> dbContextFactory)
    {
        Mapper = mapper;
        DbContextFactory = dbContextFactory;
    }

    [HttpPost]
    [ProducesResponseType(typeof(LinkGet), StatusCodes.Status201Created)]
    public IActionResult PostNewLink([FromBody] LinkCreate linkCreate)
    {
        // Map LinkCreate object (received) to new Link object
        Link link = Mapper.Map<LinkCreate, Link>(linkCreate);

        // Fill in server-generated data
        link.Created = DateTime.UtcNow;
        if (link.)

        // Second, scaffold db context
        using TrimLinkDbContext dbContext = DbContextFactory.CreateDbContext();


        // Add and attempt to save db context
        dbContext.Links.Add(link)

        throw new NotImplementedException();
    }

}