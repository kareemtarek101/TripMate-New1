
namespace TripMate.Application
{
    public interface IUserService
    {
        Task<UserDto?> GetProfileAsync(int userId);
        Task<bool> UpdateProfileAsync(int userId, UpdateUserProfileRequest request);
        Task<bool> UpdatePreferencesAsync(int userId, UpdateUserPreferenceRequest request);
        Task<UserDto?> GetCurrentUserAsync(int userId);
        Task<List<DestinationDto>> GetRecentlyViewed(int userId);
    }
}
