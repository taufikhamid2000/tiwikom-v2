using Microsoft.AspNetCore.Identity;

namespace TIWIKOM.WebApp.Services;

public class RoleDataInitializer
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<RoleDataInitializer> _logger;

    public RoleDataInitializer(RoleManager<IdentityRole> roleManager, ILogger<RoleDataInitializer> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var roles = new[] { "Admin", "Supervisor", "Employee" };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role));
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Role '{role}' created successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to create role '{role}'.");
                }
            }
        }
    }
}
