using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using trimlink.data.Repositories;

namespace trimlink.data.Models;

[Index(nameof(Token), Name = "UX_Links_Token", IsUnique = true)]
[Index(nameof(UtcDateExpires), Name = "IX_Links_UtcDateExpires")]
public class Link : IEntity<int>
{
    /// <summary>
    /// The private ID.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// DateTime that this <see cref="Link"/> was created.
    /// </summary>
    public DateTime UtcDateCreated { get; set; }

    /// <summary>
    /// DateTime when this <see cref="Link"/> is to be marked for deletion. This should be set to <see cref="DateTime.MaxValue"/> if this <see cref="Link"/> does not expire.
    /// </summary>
    public DateTime UtcDateExpires { get; set; }

    /// <summary>
    /// Set to true if this link is meant to never expire.
    /// </summary>
    public bool IsNeverExpires { get; set; }

    /// <summary>
    /// Set to true once this <see cref="Link"/> is marked for deletion.
    /// </summary>
    public bool IsMarkedForDeletion { get; set; }

    /// <summary>
    /// The randomly generated Token string. Only set once during instantiation.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// The URL to redirect to when visiting the fully-qualified <see cref="Link.TrimmedUrl"/>.
    /// </summary>
    public string RedirectToUrl { get; set; } = string.Empty;
}