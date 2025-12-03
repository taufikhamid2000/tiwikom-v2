using Microsoft.AspNetCore.Identity;

namespace TIWIKOM.Entities;

/// <summary>
/// Represents a user in the TIWIKOM application
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Gets or sets the user's first name
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the user's last name
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the date when the user account was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the user account was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    // Navigation property
    public virtual ICollection<Tip> Tips { get; set; } = new List<Tip>();
}
