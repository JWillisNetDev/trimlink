using System.ComponentModel.DataAnnotations;

namespace trimlink.website.Contracts;

public class LinkCreateDto
{
    [Required(ErrorMessage = "No URL was supplied.")]
    [Url(ErrorMessage = "The URL given was malformed or not in an accepted format.")]
    public string RedirectToUrl { get; init; } = string.Empty;
    
    [RegularExpression(@"^[0-9]+\.[0-9]{1,2}:[0-9]{1,2}:[0-9]{1,2}(\.[0-9]{1,3})?$", ErrorMessage = "The duration given was not in the correct duration format.")]
    public TimeSpan Duration { get; init; }
    public bool IsNeverExpires { get; init; }
}