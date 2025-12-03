using Microsoft.AspNetCore.Identity;
using TIWIKOM.Entities;

namespace TIWIKOM.WebApp.Services;

public class AdminUserInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AdminUserInitializer> _logger;

    public AdminUserInitializer(UserManager<ApplicationUser> userManager, ILogger<AdminUserInitializer> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        // Create default admin user
        var adminEmail = "admin@tiwikom.com";
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User"
            };

            var result = await _userManager.CreateAsync(adminUser, "Admin@123");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                _logger.LogInformation($"Admin user '{adminEmail}' created successfully.");
            }
            else
            {
                _logger.LogError($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            // Ensure admin user has Admin role
            if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                _logger.LogInformation($"Added Admin role to existing user '{adminEmail}'.");
            }
        }
    }
}
