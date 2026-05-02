public class UpdateUserPreferenceRequest
{
    public int? PreferredTripTypeId { get; set; }
    public decimal? MinBudget { get; set; }
    public decimal? MaxBudget { get; set; }
    public string? PreferredSeason { get; set; }
    public string? PreferredAirlines { get; set; }
}