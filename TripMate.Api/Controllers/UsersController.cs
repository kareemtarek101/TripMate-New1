using Microsoft.AspNetCore.Mvc;
using TripMate.Application;

namespace TripMate.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // 👤 Get Profile
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var result = await _userService.GetProfileAsync(userId);

            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }

        // ✏️ Update Profile
        [HttpPut("profile/{userId}")]
        public async Task<IActionResult> UpdateProfile(int userId, [FromBody] UpdateUserProfileRequest request)
        {
            if (request == null)
                return BadRequest("Invalid data");

            var result = await _userService.UpdateProfileAsync(userId, request);

            if (!result)
                return NotFound("User not found");

            return Ok("Profile updated successfully");
        }

        // ⚙️ Update Preferences
        [HttpPut("preferences/{userId}")]
        public async Task<IActionResult> UpdatePreferences(int userId, [FromBody] UpdateUserPreferenceRequest request)
        {
            if (request == null)
                return BadRequest("Invalid data");

            var result = await _userService.UpdatePreferencesAsync(userId, request);

            if (!result)
                return BadRequest("Failed to update preferences");

            return Ok("Preferences updated successfully");
        }

        // 👁 Recently Viewed
        [HttpGet("recent/{userId}")]
        public async Task<IActionResult> GetRecentlyViewed(int userId)
        {
            var result = await _userService.GetRecentlyViewed(userId);

            return Ok(result);
        }
    }
}