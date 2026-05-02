using TripMate.Application.Auth.Dtos;

namespace TripMate.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest request);

        Task<bool> VerifyOtp(string email, string otp);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);

        Task<string?> RefreshToken(string refreshToken);
        Task<bool> Logout(string refreshToken);
        Task<bool> ForgotPassword(string email);
        Task<bool> ResetPassword(string email, string otp, string newPassword);

        Task<bool> DeleteAccount(int userId);
    }
}
