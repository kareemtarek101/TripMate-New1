using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/favorites")]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;

    public FavoritesController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    //  Add
    [HttpPost]
    public async Task<IActionResult> Add(int userId, int itemId, string itemType)
    {
        await _favoriteService.AddFavorite(userId, itemId, itemType);

        return Ok("Added to favorites");
    }

    //  Remove
    [HttpDelete]
    public async Task<IActionResult> Remove(int userId, int itemId, string itemType)
    {
        await _favoriteService.RemoveFavorite(userId, itemId, itemType);

        return Ok("Removed from favorites");
    }

    //  Check
    [HttpGet("check")]
    public async Task<IActionResult> IsFavorite(int userId, int itemId, string itemType)
    {
        var result = await _favoriteService.IsFavorite(userId, itemId, itemType);

        return Ok(result);
    }

    //  Get All
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserFavorites(int userId)
    {
        var favorites =
            await _favoriteService.GetUserFavoritesAsync(userId);

        return Ok(favorites);
    }
}