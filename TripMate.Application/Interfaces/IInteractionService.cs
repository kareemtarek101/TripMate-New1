public interface IInteractionService
{
    Task AddInteraction(int userId, int destinationId, string type);

    Task<List<DestinationDto>> GetSuperRecommendations(int userId, decimal budget);
}