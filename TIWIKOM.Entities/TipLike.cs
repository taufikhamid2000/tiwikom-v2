namespace TIWIKOM.Entities;

/// <summary>
/// Represents a like on a tip
/// </summary>
public class TipLike
{
    public int Id { get; set; }

    /// <summary>
    /// The ID of the tip that was liked
    /// </summary>
    public int TipId { get; set; }

    /// <summary>
    /// The ID of the user who liked the tip
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// When the like was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    // Navigation properties
    public virtual Tip? Tip { get; set; }
    public virtual ApplicationUser? User { get; set; }
}
