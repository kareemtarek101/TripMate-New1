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

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        var result = await _authService.ForgotPassword(request.Email);

        if (!result)
            return BadRequest(new ApiResponse<string>("User not found"));

        return Ok(new ApiResponse<string>("OTP sent"));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var result = await _authService.ResetPassword(
            request.Email,
            request.Otp,
            request.NewPassword);

        if (!result)
            return BadRequest(new ApiResponse<string>("Invalid OTP"));

        return Ok(new ApiResponse<string>("Password reset successful"));
    }

    [Authorize]
    [HttpDelete("delete-account")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier).Value
        );

        var result = await _authService.DeleteAccount(userId);

        if (!result)
            return BadRequest(new ApiResponse<string>("User not found"));

        return Ok(new ApiResponse<string>("Account deleted"));
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        var token = await _authService.RefreshToken(request.RefreshToken);

        if (token == null)
            return Unauthorized(new ApiResponse<string>("Invalid refresh token"));

        return Ok(new ApiResponse<string>(token));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        return Ok(await _authService.RegisterAsync(request));
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpRequest request)
    {
        var result = await _authService.VerifyOtp(request.Email, request.Otp);

        if (!result)
            return BadRequest(new ApiResponse<string>("Invalid OTP"));

        return Ok(new ApiResponse<string>("Account verified successfully"));
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequest request)
    {
        var result = await _authService.Logout(request.RefreshToken);

        if (!result)
            return BadRequest(new ApiResponse<string>("Logout failed"));

        return Ok(new ApiResponse<string>("Logged out successfully"));
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);

            if (result == null)
                return Unauthorized(new ApiResponse<string>("Invalid credentials"));

            return Ok(new ApiResponse<AuthResponse>(result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<string>(ex.Message));
        }

    }
    
    public class VerifyOtpRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
