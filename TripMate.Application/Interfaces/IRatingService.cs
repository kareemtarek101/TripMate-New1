public interface IRatingService
{
    Task AddOrUpdateRating(int userId, int itemId, string itemType, int value);

    Task RemoveRating(int userId, int itemId, string itemType);

    Task<double> GetAverageRating(int itemId, string itemType);

    Task<int> GetRatingsCount(int itemId, string itemType);

    Task<int?> GetUserRating(int userId, int itemId, string itemType);
}