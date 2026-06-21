using Microsoft.EntityFrameworkCore;
using TripMate.Infrastructure.Persistence;
using TripMate.Infrastructure.Persistence.Entities;
using TripMate.Application;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripMate.Application.Interface;


namespace TripMate.Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly TripMateDbContext _context;
        private readonly ISerpApiService _serpApi;

        public RecommendationService(TripMateDbContext context, ISerpApiService serpApi)
        {
            _context = context;
            _serpApi = serpApi;
        }

        // 🧠 MAIN RECOMMENDATION
        public async Task<List<DestinationDto>> GetSmartRecommendations(int userId, decimal budget)
        {
            // 🔥 Auto Learning
            await UpdateUserPreferenceFromBehavior(userId);

            // ❤️ Favorites
            var favoriteIds = await _context.Favorites
                .Where(x => x.UserId == userId && x.ItemType == "Destination")
                .Select(x => x.ItemId)
                .ToListAsync();

            // 👁 Interactions
            var interactions = await _context.UserInteractions
                .Where(x => x.UserId == userId)
                .ToListAsync();

            // 👤 Preferences
            var pref = await _context.UserPreferences
                .Include(x => x.PreferredTripType)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            // ⭐ Ratings
            var ratings = await _context.Ratings
                .GroupBy(x => x.ItemId)
                .Select(g => new
                {
                    ItemId = g.Key,
                    Avg = g.Average(x => x.Value)
                })
                .ToListAsync();

            // 🌍 Destinations
            var destinations = await _context.Destinations
                .Where(d => d.Price <= budget)
                .ToListAsync();

            // 🧠 SCORING ENGINE
            var result = destinations
                .Select(d =>
                {
                    double score = 0;

                    // ❤️ Favorite
                    if (favoriteIds.Contains(d.DestinationId))
                        score += 5;

                    // 👁 Interaction Types
                    var userInteractions = interactions
                        .Where(x => x.DestinationId == d.DestinationId)
                        .ToList();

                    foreach (var i in userInteractions)
                    {
                        if (i.InteractionType == "View") score += 1;
                        if (i.InteractionType == "Search") score += 2;
                        if (i.InteractionType == "Booking") score += 5;
                    }

                    // ⭐ Rating
                    var rating = ratings.FirstOrDefault(r => r.ItemId == d.DestinationId)?.Avg ?? 0;
                    score += rating * 2;

                    // 🔥 Trip Type Match
                    if (pref?.PreferredTripTypeId != null &&
                        d.TripTypeId == pref.PreferredTripTypeId)
                    {
                        score += 5;
                    }

                    // 💰 Budget Smart
                    if (pref?.MinBudget != null && d.Price >= pref.MinBudget)
                        score += 1;

                    if (pref?.MaxBudget != null && d.Price <= pref.MaxBudget)
                        score += 3;

                    // 🕒 Recently Viewed Boost
                    var recentIds = interactions
                        .Where(x => x.InteractionType == "View")
                        .OrderByDescending(x => x.CreatedAt)
                        .Take(10)
                        .Select(x => x.DestinationId);

                    if (recentIds.Contains(d.DestinationId))
                        score += 2;

                    return new
                    {
                        Destination = d,
                        Score = score
                    };
                })
                .OrderByDescending(x => x.Score)
.Take(10)
.Select(x => new DestinationDto
{
    Id = x.Destination.DestinationId,
    Name = x.Destination.Name,
    Country = x.Destination.Country,
    Description = x.Destination.Description,
    ImageUrl = x.Destination.ImageUrl,
    Price = x.Destination.Price,
    DurationDays = x.Destination.DurationDays,
    Itinerary = x.Destination.Itinerary,
    Activities = x.Destination.Activities,

    City = x.Destination.City,
    AirportCode = x.Destination.AirportCode,

    Rating = 0
})
.ToList();

            return result;
        }

        // 🔥 SMART PACKAGES
        public async Task<List<PackageDto>> GetSmartPackages(int userId, decimal budget, string from)
        {
            var destinations = await GetSmartRecommendations(userId, budget);

            var packages = new List<PackageDto>();

            foreach (var dest in destinations.Take(5))
            {

                if (string.IsNullOrWhiteSpace(dest.City) ||
        string.IsNullOrWhiteSpace(dest.AirportCode))
                {
                    continue;
                }
                var hotels = await _serpApi.GetHotels(
                dest.City,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)),
                 DateOnly.FromDateTime(DateTime.UtcNow.AddDays(12))
                    );

                var flights = await _serpApi.GetFlights(
    from,
    dest.AirportCode,
    DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7))
);

                packages.Add(new PackageDto
                {
                    Destination = dest,
                    Hotels = hotels.Take(3).ToList(),
                    Flights = flights.Take(2).ToList()
                });
            }

            return packages;
        }

        // 🧠 AUTO LEARNING
        private async Task UpdateUserPreferenceFromBehavior(int userId)
        {
            var bestTripType = await GetUserPreferredTripType(userId);

            if (bestTripType == null)
                return;

            var pref = await _context.UserPreferences
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (pref == null)
            {
                _context.UserPreferences.Add(new UserPreference
                {
                    UserId = userId,
                    PreferredTripTypeId = bestTripType,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                pref.PreferredTripTypeId = bestTripType;
                pref.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        //  BEHAVIOR ANALYSIS
        private async Task<int?> GetUserPreferredTripType(int userId)
        {
            var favDestinations = await _context.Favorites
                .Where(x => x.UserId == userId && x.ItemType == "Destination")
                .Select(x => x.ItemId)
                .ToListAsync();

            if (!favDestinations.Any())
                return null;

            var tripType = await _context.Destinations
                .Where(d => favDestinations.Contains(d.DestinationId))
                .GroupBy(d => d.TripTypeId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return tripType;
        }
    }
}