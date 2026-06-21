using Microsoft.AspNetCore.Mvc;
using TripMate.Application.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IRecommendationService _recommendationService;
    private readonly IDestinationService _destinationService;

    public HomeController(
        IRecommendationService recommendationService,
        IDestinationService destinationService)
    {
        _recommendationService = recommendationService;
        _destinationService = destinationService;
    }

    // ✅ بدون userId في URL
    [HttpGet]
    public async Task<IActionResult> GetHome(
    [FromQuery] decimal budget = 2000,
    [FromQuery] string from = "CAI")
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        int userId = int.Parse(userIdClaim);

        var recommended = await _recommendationService
            .GetSmartRecommendations(userId, budget);

        var popular = await _destinationService.GetAllAsync(1, 10);

        return Ok(new
        {
            recommended,
            popular
        });
    }
}