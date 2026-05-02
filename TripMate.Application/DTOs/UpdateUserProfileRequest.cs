namespace TripMate.Application
{
    public class UpdateUserProfileRequest
    {
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? PreferredLanguage { get; set; }
        public string? PreferredCurrency { get; set; }
    }
}
