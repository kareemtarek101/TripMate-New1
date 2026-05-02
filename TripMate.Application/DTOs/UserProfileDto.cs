public class UserProfileDto
{
    public int UserId { get; set; }

    public string? PreferredTripType { get; set; }

    public decimal? MinBudget { get; set; }
    public decimal? MaxBudget { get; set; }

    public string? PreferredSeason { get; set; }

    public int TotalFavorites { get; set; }
    public int TotalBookings { get; set; }
}