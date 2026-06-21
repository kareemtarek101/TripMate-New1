using Microsoft.EntityFrameworkCore;
using TripMate.Application;

using TripMate.Infrastructure.Persistence;

namespace TripMate.Application
{
    public class UserService : IUserService
    {
        private readonly TripMateDbContext _context;

        public UserService(TripMateDbContext context)
        {
            _context = context;
        }

        // 👤 Get Full Profile (🔥 upgraded)
        public async Task<UserDto?> GetProfileAsync(int userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);

            if (user == null)
                return null;

            // 🔥 Preferences
            var pref = await _context.UserPreferences
                .Include(x => x.PreferredTripType)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            // ❤️ Favorites count
            var favoritesCount = await _context.Favorites
                .CountAsync(x => x.UserId == userId);

            // 📦 Bookings count
            var bookingsCount = await _context.Bookings
                .CountAsync(x => x.UserId == userId);

            return new UserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                ProfileImageUrl = user.ProfileImageUrl,
                PreferredLanguage = user.PreferredLanguage,
                PreferredCurrency = user.PreferredCurrency,

                // 🔥 NEW DATA
                PreferredTripType = pref?.PreferredTripType?.Name,
                MinBudget = pref?.MinBudget,
                MaxBudget = pref?.MaxBudget,
                PreferredSeason = pref?.PreferredSeason,

                TotalFavorites = favoritesCount,
                TotalBookings = bookingsCount
            };
        }

        // ✏️ Update Profile
        public async Task<bool> UpdateProfileAsync(int userId, UpdateUserProfileRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);

            if (user == null)
                return false;

            user.FullName = request.FullName;
            user.Phone = request.Phone;
            user.ProfileImageUrl = request.ProfileImageUrl;
            user.PreferredLanguage = request.PreferredLanguage;
            user.PreferredCurrency = request.PreferredCurrency;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DestinationDto>> GetRecentlyViewed(int userId)
        {
            var destinationIds = await _context.UserInteractions
                .Where(x => x.UserId == userId && x.InteractionType == "View")
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => x.DestinationId)
                .Distinct()
                .Take(10)
                .ToListAsync();

            return await _context.Destinations
                .Where(d => destinationIds.Contains(d.DestinationId))
                .Select(d => new DestinationDto
                {
                    Id = d.DestinationId,
                    Name = d.Name,
                    Country = d.Country,
                    Description = d.Description,
                    ImageUrl = d.ImageUrl,
                    Price = d.Price,
                    DurationDays = d.DurationDays,
                    Itinerary = d.Itinerary,
                    Activities = d.Activities,

                    Rating = _context.Ratings
                        .Where(r => r.ItemId == d.DestinationId
                                 && r.ItemType == "Destination")
                        .Average(r => (double?)r.Value) ?? 0
                })
                .ToListAsync();
        }


        // 🔥 Update Preferences (NEW)
        public async Task<bool> UpdatePreferencesAsync(int userId, UpdateUserPreferenceRequest request)
        {
            if (request.PreferredTripTypeId.HasValue)
            {
                var tripTypeExists = await _context.TripTypes
                    .AnyAsync(t => t.TripTypeId == request.PreferredTripTypeId);

                if (!tripTypeExists)
                    return false;
            }
            var pref = await _context.UserPreferences
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (request.PreferredTripTypeId.HasValue)
            {
                var exists = await _context.TripTypes
                    .AnyAsync(t => t.TripTypeId == request.PreferredTripTypeId);

                if (!exists)
                    return false;
            }

            if (pref == null)
            {
                pref = new Infrastructure.Persistence.Entities.UserPreference
                {
                    UserId = userId,
                    PreferredTripTypeId = request.PreferredTripTypeId,
                    MinBudget = request.MinBudget,
                    MaxBudget = request.MaxBudget,
                    PreferredSeason = request.PreferredSeason,
                    PreferredAirlines = request.PreferredAirlines,
                    CreatedAt = DateTime.UtcNow
                };

                _context.UserPreferences.Add(pref);
            }
            else
            {
                pref.PreferredTripTypeId = request.PreferredTripTypeId;
                pref.MinBudget = request.MinBudget;
                pref.MaxBudget = request.MaxBudget;
                pref.PreferredSeason = request.PreferredSeason;
                pref.PreferredAirlines = request.PreferredAirlines;
                pref.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<UserDto?> GetCurrentUserAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone
                })
                .FirstOrDefaultAsync();
        }

    }
}