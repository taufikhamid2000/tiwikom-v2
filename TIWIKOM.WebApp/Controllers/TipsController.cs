using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TIWIKOM.Entities;
using TIWIKOM.WebApp.Services;

namespace TIWIKOM.WebApp.Controllers;

public class TipsController : Controller
{
    private readonly TipService _tipService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<TipsController> _logger;

    public TipsController(TipService tipService, UserManager<ApplicationUser> userManager, ILogger<TipsController> logger)
    {
        _tipService = tipService;
        _userManager = userManager;
        _logger = logger;
    }

    // GET: Tips/Detail/5
    public async Task<IActionResult> Detail(int id)
    {
        var tip = await _tipService.GetTipByIdAsync(id);
        if (tip == null)
        {
            return NotFound();
        }

        return View(tip);
    }

    // GET: Tips/Create
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _tipService.GetAllCategoriesAsync();
        return View();
    }

    // POST: Tips/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<IActionResult> Create(Tip tip)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _tipService.GetAllCategoriesAsync();
            return View(tip);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        tip.AuthorId = user.Id;
        tip.IsPublished = true;

        await _tipService.CreateTipAsync(tip);
        return RedirectToAction("Index", "Home");
    }

    // GET: Tips/Edit/5
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<IActionResult> Edit(int id)
    {
        var tip = await _tipService.GetTipByIdAsync(id);
        if (tip == null)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        var isAdmin = User.IsInRole("Admin");

        if (tip.AuthorId != user?.Id && !isAdmin)
        {
            return Forbid();
        }

        ViewBag.Categories = await _tipService.GetAllCategoriesAsync();
        return View(tip);
    }

    // POST: Tips/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<IActionResult> Edit(int id, Tip tip)
    {
        if (id != tip.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _tipService.GetAllCategoriesAsync();
            return View(tip);
        }

        var existingTip = await _tipService.GetTipByIdAsync(id);
        if (existingTip == null)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        var isAdmin = User.IsInRole("Admin");

        if (existingTip.AuthorId != user?.Id && !isAdmin)
        {
            return Forbid();
        }

        existingTip.Title = tip.Title;
        existingTip.Content = tip.Content;
        existingTip.Abbreviation = tip.Abbreviation;
        existingTip.CategoryId = tip.CategoryId;
        existingTip.IsPublished = tip.IsPublished;

        await _tipService.UpdateTipAsync(existingTip);

        return RedirectToAction(nameof(Detail), new { id = tip.Id });
    }

    // GET: Tips/Delete/5
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<IActionResult> Delete(int id)
    {
        var tip = await _tipService.GetTipByIdAsync(id);
        if (tip == null)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        var isAdmin = User.IsInRole("Admin");

        if (tip.AuthorId != user?.Id && !isAdmin)
        {
            return Forbid();
        }

        return View(tip);
    }

    // POST: Tips/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tip = await _tipService.GetTipByIdAsync(id);
        if (tip == null)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        var isAdmin = User.IsInRole("Admin");

        if (tip.AuthorId != user?.Id && !isAdmin)
        {
            return Forbid();
        }

        await _tipService.DeleteTipAsync(id);

        TempData["Message"] = "Tip deleted successfully.";
        return RedirectToAction("Index", "Home");
    }

    // GET: Tips/MyTips
    [Authorize]
    public async Task<IActionResult> MyTips()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        var myTips = await _tipService.GetTipsByAuthorAsync(user.Id);
        return View(myTips);
    }
}
