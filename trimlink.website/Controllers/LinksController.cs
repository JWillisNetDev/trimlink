using Microsoft.AspNetCore.Mvc;
using trimlink.website.Contracts;
using AutoMapper;
using trimlink.core.Services;
using trimlink.core.Records;

namespace trimlink.website.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class LinksController : Controller
{
    private readonly ILogger<LinksController> _logger;
    private readonly IMapper _mapper;
    private readonly ILinkService _linkService;

    public LinksController(ILogger<LinksController> logger, IMapper mapper, ILinkService linkService)
    {
        _logger = logger;
        _mapper = mapper;
        _linkService = linkService;
    }

    [HttpPost(Name = "CreateLink")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateLink([FromBody] LinkCreateDto linkCreate)
    {
        string token = await _linkService.GenerateShortLink(linkCreate.RedirectToUrl, linkCreate.Duration);
        _logger.LogInformation("Generated {Token} redirects to {Url}", token, linkCreate.RedirectToUrl);
        return Created($"/api/links/{token}", token);
    }

    [HttpGet("{token}", Name = "GetLinkRedirect")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLinkRedirect([FromRoute] string token)
    {
        string? toUrl = await _linkService.GetLongUrlByToken(token);

        return toUrl is null ?
            NotFound(token) :
            Ok(toUrl);
    }

    [HttpGet("{token}/details", Name = "LinkDetails")]
    [ProducesResponseType(typeof(LinkGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLinkDetails([FromRoute] string token)
    {
        LinkDetails? link = await _linkService.GetLinkDetailsByToken(token);

        if (link is null)
            return NotFound(token);

        LinkGetDto linkDetails = _mapper.Map<LinkDetails, LinkGetDto>(link);
        return Ok(linkDetails);
    }
}