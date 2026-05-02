using Microsoft.AspNetCore.Mvc;
using TripMate.Application;
using TripMate.Application.Services;
using TripMate.Application.Interface;


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

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetHome(int userId)
    {
        // 🧠 Recommendations
        var recommended = await _recommendationService
            .GetSmartRecommendations(userId, 2000);

        // 🔥 Popular
        var popular = await _destinationService.GetAllAsync(1, 10);

        return Ok(new
        {
            recommended,
            popular
        });
    }
}