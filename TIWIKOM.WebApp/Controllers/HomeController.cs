using Microsoft.AspNetCore.Mvc;
using TIWIKOM.Entities;
using TIWIKOM.WebApp.Services;

namespace TIWIKOM.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly TipService _tipService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(TipService tipService, ILogger<HomeController> logger)
    {
        _tipService = tipService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, int? categoryId = null, string? search = null, string? sortBy = "date")
    {
        try
        {
            var categories = await _tipService.GetAllCategoriesAsync();

            var (tips, totalCount) = await _tipService.SearchTipsAsync(
                searchTerm: search,
                categoryId: categoryId,
                sortBy: sortBy,
                page: pageNumber,
                pageSize: 10
            );

            var totalPages = (int)Math.Ceiling(totalCount / 10.0);

            ViewBag.Categories = categories;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SearchTerm = search;
            ViewBag.SortBy = sortBy;

            return View(tips);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tips");
            return View("Error");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> Search(string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return RedirectToAction(nameof(Index));
        }

        var (tips, totalCount) = await _tipService.SearchTipsAsync(searchTerm: q, page: 1, pageSize: 50);
        var categories = await _tipService.GetAllCategoriesAsync();

        ViewBag.SearchQuery = q;
        ViewBag.TotalResults = totalCount;
        ViewBag.Categories = categories;

        return View(tips);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
