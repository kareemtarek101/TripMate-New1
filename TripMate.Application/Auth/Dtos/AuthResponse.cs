namespace TripMate.Application.Auth.Dtos
{
    public class AuthResponse
    {
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; }
    }
}
