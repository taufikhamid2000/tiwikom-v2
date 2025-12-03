using TIWIKOM.Entities;
using TIWIKOM.Entities.Contexts;
using Microsoft.EntityFrameworkCore;

namespace TIWIKOM.WebApp.Services;

/// <summary>
/// Service for managing tips
/// </summary>
public class TipService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TipService> _logger;

    public TipService(ApplicationDbContext context, ILogger<TipService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all published tips with pagination
    /// </summary>
    public async Task<(List<Tip> tips, int totalCount)> GetPublishedTipsAsync(int page = 1, int pageSize = 10)
    {
        var query = _context.Tips
            .Where(t => t.IsPublished)
            .Include(t => t.Author)
            .Include(t => t.Category)
            .OrderByDescending(t => t.CreatedDate);

        var totalCount = await query.CountAsync();
        var tips = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (tips, totalCount);
    }

    /// <summary>
    /// Get a single tip by ID
    /// </summary>
    public async Task<Tip?> GetTipByIdAsync(int id)
    {
        var tip = await _context.Tips
            .Include(t => t.Author)
            .Include(t => t.Category)
            .Include(t => t.Comments)
            .Include(t => t.Likes)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tip != null && tip.IsPublished)
        {
            tip.ViewCount++;
            await _context.SaveChangesAsync();
        }

        return tip;
    }

    /// <summary>
    /// Create a new tip
    /// </summary>
    public async Task<Tip> CreateTipAsync(Tip tip)
    {
        tip.CreatedDate = DateTime.UtcNow;
        tip.ViewCount = 0;
        _context.Tips.Add(tip);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Tip '{tip.Title}' created by user {tip.AuthorId}");
        return tip;
    }

    /// <summary>
    /// Update an existing tip
    /// </summary>
    public async Task<Tip> UpdateTipAsync(Tip tip)
    {
        tip.ModifiedDate = DateTime.UtcNow;
        _context.Tips.Update(tip);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Tip '{tip.Title}' updated by user {tip.AuthorId}");
        return tip;
    }

    /// <summary>
    /// Delete a tip
    /// </summary>
    public async Task DeleteTipAsync(int id)
    {
        var tip = await _context.Tips.FindAsync(id);
        if (tip != null)
        {
            _context.Tips.Remove(tip);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Tip {id} deleted");
        }
    }

    /// <summary>
    /// Get tips by category
    /// </summary>
    public async Task<List<Tip>> GetTipsByCategoryAsync(int categoryId)
    {
        return await _context.Tips
            .Where(t => t.CategoryId == categoryId && t.IsPublished)
            .Include(t => t.Author)
            .Include(t => t.Category)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .OrderBy(c => c.SortOrder)
            .ToListAsync();
    }

    /// <summary>
    /// Get tips by author ID
    /// </summary>
    public async Task<List<Tip>> GetTipsByAuthorAsync(string authorId)
    {
        return await _context.Tips
            .Where(t => t.AuthorId == authorId)
            .Include(t => t.Author)
            .Include(t => t.Category)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    /// <summary>
    /// Search tips with filters
    /// </summary>
    public async Task<(List<Tip> tips, int totalCount)> SearchTipsAsync(
        string? searchTerm = null,
        int? categoryId = null,
        string? sortBy = "date",
        int page = 1,
        int pageSize = 10)
    {
        var query = _context.Tips
            .Where(t => t.IsPublished)
            .Include(t => t.Author)
            .Include(t => t.Category)
            .Include(t => t.Likes)
            .Include(t => t.Comments)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(t =>
                t.Title.ToLower().Contains(searchTerm) ||
                t.Content.ToLower().Contains(searchTerm) ||
                t.Abbreviation.ToLower().Contains(searchTerm) ||
                (t.Author != null && (t.Author.FirstName.ToLower().Contains(searchTerm) || t.Author.LastName.ToLower().Contains(searchTerm)))
            );
        }

        // Apply category filter
        if (categoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == categoryId.Value);
        }

        // Apply sorting
        query = sortBy?.ToLower() switch
        {
            "popular" => query.OrderByDescending(t => t.Likes.Count),
            "views" => query.OrderByDescending(t => t.ViewCount),
            "comments" => query.OrderByDescending(t => t.Comments.Count),
            "title" => query.OrderBy(t => t.Title),
            _ => query.OrderByDescending(t => t.CreatedDate), // Default: newest first
        };

        var totalCount = await query.CountAsync();
        var tips = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (tips, totalCount);
    }
}
