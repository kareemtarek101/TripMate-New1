using TripMate.Infrastructure.Persistence;
using TripMate.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
public class FavoriteService : IFavoriteService
{
    private readonly TripMateDbContext _context;

    public FavoriteService(TripMateDbContext context)
    {
        _context = context;
    }

    public async Task AddFavorite(int userId, int itemId, string itemType)
    {
        var exists = await _context.Favorites
            .AnyAsync(x => x.UserId == userId && x.ItemId == itemId && x.ItemType == itemType);

        if (!exists)
        {
            var fav = new Favorite
            {
                UserId = userId,
                ItemId = itemId,
                ItemType = itemType,
                CreatedAt = DateTime.UtcNow
            };

            _context.Favorites.Add(fav);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveFavorite(int userId, int itemId, string itemType)
    {
        var fav = await _context.Favorites
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ItemId == itemId && x.ItemType == itemType);

        if (fav != null)
        {
            _context.Favorites.Remove(fav);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsFavorite(int userId, int itemId, string itemType)
    {
        return await _context.Favorites
            .AnyAsync(x => x.UserId == userId && x.ItemId == itemId && x.ItemType == itemType);
    }

    public async Task<List<Favorite>> GetUserFavorites(int userId)
    {
        return await _context.Favorites
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }
}