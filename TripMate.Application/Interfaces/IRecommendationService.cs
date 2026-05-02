
namespace TripMate.Application.Interface
{
    public interface IRecommendationService
    {
        Task<List<DestinationDto>> GetSmartRecommendations(int userId, decimal budget);

        Task<List<PackageDto>> GetSmartPackages(int userId, decimal budget, string from);
    }
}