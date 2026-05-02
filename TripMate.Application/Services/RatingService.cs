using Microsoft.EntityFrameworkCore;
using TripMate.Infrastructure.Persistence;
using TripMate.Infrastructure.Persistence.Entities;

public class RatingService : IRatingService
{
    private readonly TripMateDbContext _context;

    public RatingService(TripMateDbContext context)
    {
        _context = context;
    }

    // ➕ Add or Update
    public async Task AddOrUpdateRating(int userId, int itemId, string itemType, int value)
    {
        if (value < 1 || value > 5)
            throw new Exception("Rating must be between 1 and 5");

        var existing = await _context.Ratings
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.ItemId == itemId &&
                x.ItemType == itemType);

        if (existing != null)
        {
            existing.Value = value;
            existing.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            var rating = new Rating
            {
                UserId = userId,
                ItemId = itemId,
                ItemType = itemType,
                Value = value,
                CreatedAt = DateTime.UtcNow
            };

            _context.Ratings.Add(rating);
        }

        await _context.SaveChangesAsync();
    }

    // ❌ Remove
    public async Task RemoveRating(int userId, int itemId, string itemType)
    {
        var rating = await _context.Ratings
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.ItemId == itemId &&
                x.ItemType == itemType);

        if (rating != null)
        {
            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
        }
    }

    // ⭐ Average
    public async Task<double> GetAverageRating(int itemId, string itemType)
    {
        return await _context.Ratings
            .Where(x => x.ItemId == itemId && x.ItemType == itemType)
            .AverageAsync(x => (double?)x.Value) ?? 0;
    }

    // 🔢 Count
    public async Task<int> GetRatingsCount(int itemId, string itemType)
    {
        return await _context.Ratings
            .CountAsync(x => x.ItemId == itemId && x.ItemType == itemType);
    }

    // 👤 User rating
    public async Task<int?> GetUserRating(int userId, int itemId, string itemType)
    {
        return await _context.Ratings
            .Where(x => x.UserId == userId && x.ItemId == itemId && x.ItemType == itemType)
            .Select(x => (int?)x.Value)
            .FirstOrDefaultAsync();
    }
}