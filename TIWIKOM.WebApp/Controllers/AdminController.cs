using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TIWIKOM.Entities;
using TIWIKOM.WebApp.Models;

namespace TIWIKOM.WebApp.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<AdminController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    // GET: Admin/Index
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        var userViewModels = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userViewModels.Add(new UserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? "",
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList()
            });
        }

        return View(userViewModels);
    }

    // GET: Admin/ManageRoles/userId
    public async Task<IActionResult> ManageRoles(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var allRoles = await _roleManager.Roles.ToListAsync();

        var model = new ManageRolesViewModel
        {
            UserId = user.Id,
            UserEmail = user.Email ?? "",
            UserRoles = allRoles.Select(r => new RoleSelection
            {
                RoleName = r.Name ?? "",
                IsSelected = userRoles.Contains(r.Name ?? "")
            }).ToList()
        };

        return View(model);
    }

    // POST: Admin/ManageRoles
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ManageRoles(ManageRolesViewModel model)
    {
        if (string.IsNullOrEmpty(model.UserId))
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        // Remove all existing roles
        var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
        if (!removeResult.Succeeded)
        {
            ModelState.AddModelError("", "Failed to remove existing roles");
            return View(model);
        }

        // Add selected roles
        var selectedRoles = model.UserRoles.Where(r => r.IsSelected).Select(r => r.RoleName).ToList();
        if (selectedRoles.Any())
        {
            var addResult = await _userManager.AddToRolesAsync(user, selectedRoles);
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add selected roles");
                return View(model);
            }
        }

        TempData["Message"] = $"Roles updated successfully for {user.Email}";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/DeleteUser/userId
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var model = new UserViewModel
        {
            Id = user.Id,
            Email = user.Email ?? "",
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles.ToList()
        };

        return View(model);
    }

    // POST: Admin/DeleteUser
    [HttpPost, ActionName("DeleteUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUserConfirmed(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Prevent deleting yourself
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser?.Id == user.Id)
        {
            TempData["Error"] = "You cannot delete your own account";
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            TempData["Message"] = $"User {user.Email} deleted successfully";
        }
        else
        {
            TempData["Error"] = $"Failed to delete user: {string.Join(", ", result.Errors.Select(e => e.Description))}";
        }

        return RedirectToAction(nameof(Index));
    }
}
