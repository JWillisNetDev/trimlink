using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace trimlink.data.Models;

[Index(nameof(Token), Name = "UX_Links_Token", IsUnique = true)]
[Index(nameof(UtcDateExpires), Name = "IX_Links_UtcDateExpires")]
public class Link
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
    /// DateTime when this <see cref="Link"/> is to be marked for deletion. This should be set to null if it does not expire.
    /// </summary>
    [ReadOnly(true)]
    public DateTime? UtcDateExpires { get; set; }

    /// <summary>
    /// Getter only. True if this link never expires. Checks if <see cref="UtcDateExpires"/> is null.
    /// </summary>
    public bool IsNeverExpires => UtcDateExpires is null;

    /// <summary>
    /// Set to true once this <see cref="Link"/> is marked for deletion.
    /// </summary>
    public bool IsMarkedForDeletion { get; set; }

    /// <summary>
    /// The randomly generated Token string. Only set once during instantiation.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// The URL to redirect to.
    /// </summary>
    public string RedirectToUrl { get; set; } = string.Empty;
}