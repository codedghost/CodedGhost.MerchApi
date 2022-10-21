using CodedGhost.MerchApi.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodedGhost.MerchApi.Controllers;

[Route("[controller]")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetCategories().ConfigureAwait(false);

        return Json(categories);
    }
}