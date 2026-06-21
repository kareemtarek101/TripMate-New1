using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripMate.Application.Auth.Dtos;
using TripMate.Application.Auth.Interfaces;
using TripMate.Infrastructure.Persistence.Entities;

namespace TripMate.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // =========================================
    // REGISTER
    // =========================================
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);

        if (result.Token == "EMAIL_ALREADY_EXISTS")
            return BadRequest("Email already exists");

        return Ok(result);
    }

    // =========================================
    // LOGIN
    // =========================================
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result == null)
            return Unauthorized(
                new ApiResponse<string>("Invalid credentials"));

        return Ok(new ApiResponse<AuthResponse>(result));
    }

    // =========================================
    // REFRESH TOKEN
    // =========================================
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        var token = await _authService.RefreshToken(request.RefreshToken);

        if (token == null)
            return Unauthorized(
                new ApiResponse<string>("Invalid refresh token"));

        return Ok(new ApiResponse<string>(token));
    }

    // =========================================
    // LOGOUT
    // =========================================
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequest request)
    {
        var result = await _authService.Logout(request.RefreshToken);

        if (!result)
            return BadRequest(
                new ApiResponse<string>("Logout failed"));

        return Ok(
            new ApiResponse<string>("Logged out successfully"));
    }

    // =========================================
    // FORGOT PASSWORD
    // =========================================
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
        ForgotPasswordRequest request)
    {
        var result =
            await _authService.ForgotPassword(request.Email);

        if (!result)
            return BadRequest(
                new ApiResponse<string>("User not found"));

        return Ok(
            new ApiResponse<string>("OTP sent"));
    }

    // =========================================
    // VERIFY OTP
    // =========================================
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(
        VerifyOtpRequest request)
    {
        var result =
            await _authService.VerifyOtp(
                request.Email,
                request.Otp);

        if (!result)
            return BadRequest(
                new ApiResponse<string>("Invalid OTP"));

        return Ok(
            new ApiResponse<string>(
                "Account verified successfully"));
    }

    // =========================================
    // RESET PASSWORD
    // =========================================
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        ResetPasswordRequest request)
    {
        var result =
            await _authService.ResetPassword(
                request.Email,
                request.Otp,
                request.NewPassword);

        if (!result)
            return BadRequest(
                new ApiResponse<string>("Invalid OTP"));

        return Ok(
            new ApiResponse<string>(
                "Password reset successful"));
    }

    // =========================================
    // DELETE ACCOUNT
    // =========================================
    [Authorize]
    [HttpDelete("delete-account")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized(
                new ApiResponse<string>("Invalid token"));

        var userId = int.Parse(userIdClaim.Value);

        var result =
            await _authService.DeleteAccount(userId);

        if (!result)
            return BadRequest(
                new ApiResponse<string>("User not found"));

        return Ok(
            new ApiResponse<string>(
                "Account deleted"));
    }

    // =========================================
    // VERIFY OTP DTO
    // =========================================
    public class VerifyOtpRequest
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}