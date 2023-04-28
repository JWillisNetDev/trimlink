using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trimlink.website.Contracts;
using AutoMapper;
using shortid;
using shortid.Configuration;
using trimlink.core.Services;
using trimlink.core.Records;

namespace trimlink.website.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class LinksController : Controller
{
    private readonly GenerationOptions _generationOptions
        = new GenerationOptions(useNumbers: true, useSpecialCharacters: false, length: 18);

    private readonly ILogger<LinksController> _logger;
    private readonly IMapper _mapper;
    private readonly ILinkService _linkService;

    public LinksController(ILogger<LinksController> logger, IMapper mapper, ILinkService linkService)
    {
        _logger = logger;
        _mapper = mapper;
        _linkService = linkService;
    }

    private string GenerateShortId()
        => ShortId.Generate(_generationOptions);

    [HttpPost(Name = "CreateLink")]
    [ProducesResponseType(typeof(LinkGetDto), StatusCodes.Status201Created)]
    public IActionResult CreateLink([FromBody] LinkCreateDto linkCreate)
    {
        int id;
        string token;
        if (linkCreate.IsNeverExpires)
        {
            token = _linkService.GenerateShortLink(linkCreate.RedirectToUrl, out id);
        }
        else
        {
            token = _linkService.GenerateShortLink(linkCreate.RedirectToUrl, linkCreate.Duration, out id);
        }

        _logger.LogInformation("Generated {token} redirect to {url}", token, linkCreate.RedirectToUrl);

        return Created(Url?.Link("RedirectTo", new { token }) ?? string.Empty, token);
    }

    [HttpGet("{token}", Name = "RedirectTo")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult RedirectFromLink([FromRoute] string token)
    {
        string? toUrl = _linkService.GetLongUrlByToken(token);

        return toUrl is null ?
            NotFound(token) :
            Ok(toUrl);
    }

    [HttpGet("{token}/details", Name = "LinkDetails")]
    [ProducesResponseType(typeof(LinkGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult GetLinkDetails([FromRoute] string token)
    {
        var link = _linkService.GetLinkDetailsByToken(token);

        if (link is null)
            return NotFound(token);

        LinkGetDto linkDetails = _mapper.Map<LinkDetails, LinkGetDto>(link);
        return Ok(linkDetails);
    }
}