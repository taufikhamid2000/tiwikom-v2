using TIWIKOM.Entities;
using TIWIKOM.Entities.Contexts;

namespace TIWIKOM.WebApp.Services;

public class TipDataInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TipDataInitializer> _logger;

    public TipDataInitializer(ApplicationDbContext context, ILogger<TipDataInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Initialize categories if they don't exist
            if (!_context.Categories.Any())
            {
                var categories = new[]
                {
                    new Category { Name = "First Day", Description = "Tips for your first day at the company", SortOrder = 1 },
                    new Category { Name = "Technical", Description = "Technical knowledge and best practices", SortOrder = 2 },
                    new Category { Name = "Culture", Description = "Company culture and values", SortOrder = 3 },
                    new Category { Name = "Career Growth", Description = "Tips for career development", SortOrder = 4 },
                    new Category { Name = "Communication", Description = "Communication best practices", SortOrder = 5 }
                };

                _context.Categories.AddRange(categories);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Categories initialized successfully.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing tip categories.");
        }
    }
}
