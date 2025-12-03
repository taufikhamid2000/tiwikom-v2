namespace TIWIKOM.Entities;

/// <summary>
/// Represents a category for tips (e.g., "First Day", "Technical", "Culture")
/// </summary>
public class Category
{
    public int Id { get; set; }

    /// <summary>
    /// Name of the category
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of what tips fall under this category
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Sort order for displaying categories
    /// </summary>
    public int SortOrder { get; set; }

    // Navigation property
    public virtual ICollection<Tip> Tips { get; set; } = new List<Tip>();
}
