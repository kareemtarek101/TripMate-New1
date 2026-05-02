using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/packages")]
public class PackagesController : ControllerBase
{
    private readonly ISerpApiService _serpService;

    public PackagesController(ISerpApiService serpService)
    {
        _serpService = serpService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string from, string to)
    {
        var result = await _serpService.GetPackage(from, to);

        return Ok(new ApiResponse<PackageDto>(result));
    }
}