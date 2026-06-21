using Microsoft.AspNetCore.Mvc;
using TripMate.Application.Interface;

[ApiController]
[Route("api/packages")]
public class PackagesController : ControllerBase
{
    private readonly IPackageService _packageService;
    private readonly ISerpApiService _serpService;
    private readonly IRecommendationService _recommendationService;

    public PackagesController(
        IPackageService packageService,
        ISerpApiService serpService,
        IRecommendationService recommendationService)
    {
        _packageService = packageService;
        _serpService = serpService;
        _recommendationService = recommendationService;
    }

    // 🔥 1. GET DB Packages
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _packageService.GetAllAsync();
        return Ok(new ApiResponse<List<PackageDto>>(data));
    }

    // 🔥 2. GET Package By Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var data = await _packageService.GetByIdAsync(id);
        if (data == null) return NotFound();

        return Ok(new ApiResponse<PackageDto>(data));
    }

    // 🔥 3. External Packages (Flights + Hotels)
    [HttpGet("external")]
    public async Task<IActionResult> GetExternal(
        [FromQuery] string from,
        [FromQuery] string to)
    {
        var result = await _serpService.GetPackage(from, to);
        return Ok(new ApiResponse<PackageDto>(result));
    }

    // 🔥 4. Smart AI Packages
    [HttpGet("smart")]
    public async Task<IActionResult> GetSmart(
        [FromQuery] int userId = 1,
        [FromQuery] decimal budget = 2000,
        [FromQuery] string from = "CAI")
    {
        var result = await _recommendationService
            .GetSmartPackages(userId, budget, from);

        return Ok(result);
    }

    // 🔥 5. CREATE
    [HttpPost]
    public async Task<IActionResult> Create(CreatePackageRequest request)
    {
        var id = await _packageService.CreateAsync(request);
        return Ok(new { packageId = id });
    }

    // 🔥 6. UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePackageRequest request)
    {
        var ok = await _packageService.UpdateAsync(id, request);
        if (!ok) return NotFound();

        return Ok();
    }

    // 🔥 7. DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _packageService.DeleteAsync(id);
        if (!ok) return NotFound();

        return Ok();
    }
}