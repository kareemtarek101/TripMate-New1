using Microsoft.AspNetCore.Mvc;
using TripMate.Application;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // ✅ GET ALL
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _categoryService.GetAllAsync();
        return Ok(new ApiResponse<List<CategoryDto>>(data));
    }

    // ✅ POST CREATE
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Category name is required");

        var id = await _categoryService.CreateAsync(request);

        return Ok(new ApiResponse<object>(new
        {
            message = "Category created successfully",
            categoryId = id
        }));
    }
}