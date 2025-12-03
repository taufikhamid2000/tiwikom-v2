namespace TIWIKOM.Entities;

/// <summary>
/// Represents a comment on a tip
/// </summary>
public class Comment
{
    public int Id { get; set; }

    /// <summary>
    /// The content of the comment
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// When the comment was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// When the comment was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// The ID of the tip this comment belongs to
    /// </summary>
    public int TipId { get; set; }

    /// <summary>
    /// The ID of the user who posted this comment
    /// </summary>
    public string AuthorId { get; set; } = string.Empty;

    // Navigation properties
    public virtual Tip? Tip { get; set; }
    public virtual ApplicationUser? Author { get; set; }
}
