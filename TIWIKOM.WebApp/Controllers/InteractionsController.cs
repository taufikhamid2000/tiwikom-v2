using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TIWIKOM.Entities;
using TIWIKOM.WebApp.Services;

namespace TIWIKOM.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InteractionsController : ControllerBase
{
    private readonly InteractionService _interactionService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<InteractionsController> _logger;

    public InteractionsController(
        InteractionService interactionService,
        UserManager<ApplicationUser> userManager,
        ILogger<InteractionsController> logger)
    {
        _interactionService = interactionService;
        _userManager = userManager;
        _logger = logger;
    }

    // POST: api/interactions/like/5
    [HttpPost("like/{tipId}")]
    [Authorize]
    public async Task<IActionResult> ToggleLike(int tipId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        try
        {
            var isLiked = await _interactionService.ToggleLikeAsync(tipId, user.Id);
            var likeCount = await _interactionService.GetLikeCountAsync(tipId);

            return Ok(new { isLiked, likeCount });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error toggling like for tip {tipId}");
            return StatusCode(500, "An error occurred");
        }
    }

    // GET: api/interactions/like/5
    [HttpGet("like/{tipId}")]
    public async Task<IActionResult> GetLikeInfo(int tipId)
    {
        try
        {
            var likeCount = await _interactionService.GetLikeCountAsync(tipId);
            bool isLiked = false;

            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    isLiked = await _interactionService.HasUserLikedTipAsync(tipId, user.Id);
                }
            }

            return Ok(new { isLiked, likeCount });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting like info for tip {tipId}");
            return StatusCode(500, "An error occurred");
        }
    }

    // POST: api/interactions/comment
    [HttpPost("comment")]
    [Authorize]
    public async Task<IActionResult> AddComment([FromBody] AddCommentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest("Comment content cannot be empty");
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        try
        {
            var comment = await _interactionService.AddCommentAsync(request.TipId, user.Id, request.Content);
            
            return Ok(new
            {
                id = comment.Id,
                content = comment.Content,
                authorName = $"{comment.Author?.FirstName} {comment.Author?.LastName}",
                createdDate = comment.CreatedDate.ToString("MMM dd, yyyy 'at' h:mm tt")
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error adding comment to tip {request.TipId}");
            return StatusCode(500, "An error occurred");
        }
    }

    // DELETE: api/interactions/comment/5
    [HttpDelete("comment/{commentId}")]
    [Authorize]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        try
        {
            var isAdmin = User.IsInRole("Admin");
            var success = await _interactionService.DeleteCommentAsync(commentId, user.Id, isAdmin);

            if (!success)
            {
                return Forbid();
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting comment {commentId}");
            return StatusCode(500, "An error occurred");
        }
    }

    // GET: api/interactions/comments/5
    [HttpGet("comments/{tipId}")]
    public async Task<IActionResult> GetComments(int tipId)
    {
        try
        {
            var comments = await _interactionService.GetCommentsForTipAsync(tipId);
            var currentUserId = User.Identity?.IsAuthenticated == true
                ? (await _userManager.GetUserAsync(User))?.Id
                : null;

            var result = comments.Select(c => new
            {
                id = c.Id,
                content = c.Content,
                authorName = $"{c.Author?.FirstName} {c.Author?.LastName}",
                authorId = c.AuthorId,
                createdDate = c.CreatedDate.ToString("MMM dd, yyyy 'at' h:mm tt"),
                canDelete = currentUserId == c.AuthorId || User.IsInRole("Admin")
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting comments for tip {tipId}");
            return StatusCode(500, "An error occurred");
        }
    }
}

public class AddCommentRequest
{
    public int TipId { get; set; }
    public string Content { get; set; } = "";
}
