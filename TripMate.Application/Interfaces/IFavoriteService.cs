using TripMate.Infrastructure.Persistence.Entities;

public interface IFavoriteService
{
    Task AddFavorite(int userId, int itemId, string itemType);

    Task RemoveFavorite(int userId, int itemId, string itemType);

    Task<bool> IsFavorite(int userId, int itemId, string itemType);

    Task<List<Favorite>> GetUserFavorites(int userId);
}