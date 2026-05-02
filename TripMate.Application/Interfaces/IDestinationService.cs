using TripMate.Application;
public interface IDestinationService
{
    Task<List<DestinationDto>> GetAllAsync(int page, int pageSize);
    Task<DestinationDto?> GetByIdAsync(int id);
    Task<List<DestinationDto>> SearchAsync(string query);
    Task<List<DestinationDto>> FilterAsync(string? country, string? category, decimal? budget);
    Task<bool> CreateAsync(CreateDestinationRequest request);

}