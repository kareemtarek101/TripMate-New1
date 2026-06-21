using TripMate.Infrastructure.Persistence.Entities;

public interface IPostService
{
    Task<int> CreateAsync(int userId, CreatePostRequest request);
    Task<List<CreatePostRequest>> GetAllAsync();
    Task<CreatePostRequest?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(int id, CreatePostRequest request);
}