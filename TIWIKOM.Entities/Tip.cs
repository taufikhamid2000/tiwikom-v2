namespace TIWIKOM.Entities;

/// <summary>
/// Represents a tip or piece of advice posted by admin/senior/supervisor
/// </summary>
public class Tip
{
    public int Id { get; set; }

    /// <summary>
    /// The title of the tip (e.g., "Always ask questions")
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The full content of the tip
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Abbreviation explanation (completes "Things I Wish I Knew On My ...")
    /// </summary>
    public string Abbreviation { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the user who posted this tip
    /// </summary>
    public string AuthorId { get; set; } = string.Empty;

    /// <summary>
    /// The category of this tip
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// When the tip was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// When the tip was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Whether this tip is published and visible to employees
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Number of times this tip has been viewed
    /// </summary>
    public int ViewCount { get; set; }

    // Navigation properties
    public virtual ApplicationUser? Author { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<TipLike> Likes { get; set; } = new List<TipLike>();
}
