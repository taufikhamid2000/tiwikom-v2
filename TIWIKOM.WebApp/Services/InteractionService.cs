using TIWIKOM.Entities;
using TIWIKOM.Entities.Contexts;
using Microsoft.EntityFrameworkCore;

namespace TIWIKOM.WebApp.Services;

/// <summary>
/// Service for managing comments and likes
/// </summary>
public class InteractionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<InteractionService> _logger;

    public InteractionService(ApplicationDbContext context, ILogger<InteractionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Comments

    /// <summary>
    /// Get all comments for a tip
    /// </summary>
    public async Task<List<Comment>> GetCommentsForTipAsync(int tipId)
    {
        return await _context.Comments
            .Where(c => c.TipId == tipId)
            .Include(c => c.Author)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }

    /// <summary>
    /// Add a comment to a tip
    /// </summary>
    public async Task<Comment> AddCommentAsync(int tipId, string authorId, string content)
    {
        var comment = new Comment
        {
            TipId = tipId,
            AuthorId = authorId,
            Content = content,
            CreatedDate = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        // Load the author for the return value
        await _context.Entry(comment).Reference(c => c.Author).LoadAsync();

        _logger.LogInformation($"Comment added to tip {tipId} by user {authorId}");
        return comment;
    }

    /// <summary>
    /// Delete a comment
    /// </summary>
    public async Task<bool> DeleteCommentAsync(int commentId, string userId, bool isAdmin = false)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null) return false;

        // Only the author or an admin can delete
        if (comment.AuthorId != userId && !isAdmin)
        {
            return false;
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Comment {commentId} deleted");
        return true;
    }

    /// <summary>
    /// Get comment count for a tip
    /// </summary>
    public async Task<int> GetCommentCountAsync(int tipId)
    {
        return await _context.Comments.CountAsync(c => c.TipId == tipId);
    }

    #endregion

    #region Likes

    /// <summary>
    /// Toggle like on a tip (like if not liked, unlike if already liked)
    /// </summary>
    public async Task<bool> ToggleLikeAsync(int tipId, string userId)
    {
        var existingLike = await _context.TipLikes
            .FirstOrDefaultAsync(tl => tl.TipId == tipId && tl.UserId == userId);

        if (existingLike != null)
        {
            // Unlike
            _context.TipLikes.Remove(existingLike);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {userId} unliked tip {tipId}");
            return false; // Unliked
        }
        else
        {
            // Like
            var like = new TipLike
            {
                TipId = tipId,
                UserId = userId,
                CreatedDate = DateTime.UtcNow
            };

            _context.TipLikes.Add(like);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {userId} liked tip {tipId}");
            return true; // Liked
        }
    }

    /// <summary>
    /// Check if a user has liked a tip
    /// </summary>
    public async Task<bool> HasUserLikedTipAsync(int tipId, string userId)
    {
        return await _context.TipLikes
            .AnyAsync(tl => tl.TipId == tipId && tl.UserId == userId);
    }

    /// <summary>
    /// Get like count for a tip
    /// </summary>
    public async Task<int> GetLikeCountAsync(int tipId)
    {
        return await _context.TipLikes.CountAsync(tl => tl.TipId == tipId);
    }

    /// <summary>
    /// Get likes with user info for a tip
    /// </summary>
    public async Task<List<TipLike>> GetLikesForTipAsync(int tipId)
    {
        return await _context.TipLikes
            .Where(tl => tl.TipId == tipId)
            .Include(tl => tl.User)
            .OrderByDescending(tl => tl.CreatedDate)
            .ToListAsync();
    }

    #endregion
}
