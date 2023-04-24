using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trimlink.data;
using trimlink.data.Models;
using trimlink.website.Contracts;
using AutoMapper;
using shortid;
using shortid.Configuration;
using trimlink.data.Repositories;
using trimlink.core.Services;

namespace trimlink.website.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class LinksController : Controller
{
    private readonly GenerationOptions _generationOptions
        = new GenerationOptions(useNumbers: true, useSpecialCharacters: false, length: 18);

    private readonly IMapper _mapper;
    private readonly ILinkService _linkService;

    public LinksController(IMapper mapper, ILinkService linkService)
    {
        _mapper = mapper;
        _linkService = linkService;
    }

    private string GenerateShortId()
        => ShortId.Generate(_generationOptions);

    [HttpPost("create")]
    [ProducesResponseType(typeof(LinkGetDto), StatusCodes.Status201Created)]
    public IActionResult CreateLink([FromBody] LinkCreateDto linkCreate)
    {
        int id;
        string shortId;
        if (linkCreate.IsNeverExpires)
        {
            shortId = _linkService.GenerateShortLink(linkCreate.RedirectToUrl, out id);
        }
        else
        {
            TimeSpan expiresAfter = TimeSpan.Parse(linkCreate.Duration);
            shortId = _linkService.GenerateShortLink(linkCreate.RedirectToUrl, expiresAfter, out id);
        }

        return Created(Url?.Link("RedirectTo", new { shortId }) ?? string.Empty, shortId);
    }

    [HttpGet("~/to/{shortId}", Name = "RedirectTo")]
    public IActionResult RedirectFromLink([FromRoute] string shortId)
    {
        string? toUrl = _linkService.GetLongUrlByToken(shortId);

        return toUrl is null ?
            NotFound() :
            Redirect(toUrl);
    }
}