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
using Microsoft.AspNetCore.Cors;

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

    [HttpGet("~/to/{shortId}", Name = "RedirectTo")]
    [ProducesResponseType(typeof(string), StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult RedirectFromLink([FromRoute] string shortId)
    {
        string? toUrl = _linkService.GetLongUrlByToken(shortId);

        return toUrl is null ?
            NotFound() :
            Redirect(toUrl);
    }
}