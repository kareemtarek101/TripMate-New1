using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/ratings")]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    // ➕ Add or Update
    [HttpPost]
    public async Task<IActionResult> Add(int userId, int itemId, string itemType, int value)
    {
        await _ratingService.AddOrUpdateRating(userId, itemId, itemType, value);

        return Ok("Rating saved");
    }

    // ❌ Remove
    [HttpDelete]
    public async Task<IActionResult> Remove(int userId, int itemId, string itemType)
    {
        await _ratingService.RemoveRating(userId, itemId, itemType);

        return Ok("Rating removed");
    }

    // ⭐ Average
    [HttpGet("average")]
    public async Task<IActionResult> GetAverage(int itemId, string itemType)
    {
        var avg = await _ratingService.GetAverageRating(itemId, itemType);

        return Ok(avg);
    }

    // 🔢 Count
    [HttpGet("count")]
    public async Task<IActionResult> GetCount(int itemId, string itemType)
    {
        var count = await _ratingService.GetRatingsCount(itemId, itemType);

        return Ok(count);
    }

    // 👤 User rating
    [HttpGet("user")]
    public async Task<IActionResult> GetUserRating(int userId, int itemId, string itemType)
    {
        var rating = await _ratingService.GetUserRating(userId, itemId, itemType);

        return Ok(rating);
    }
}