using Microsoft.EntityFrameworkCore;
using TripMate.Application.Auth.Dtos;
using TripMate.Application.Auth.Interfaces;
using TripMate.Application.Services;
using TripMate.Infrastructure.Persistence;
using TripMate.Infrastructure.Persistence.Entities;
using TripMate.Infrastructure.Security;

namespace TripMate.Application.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly TripMateDbContext _context;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;

        public AuthService(
            TripMateDbContext context,
            JwtService jwtService, EmailService emailService)
        {
            _context = context;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        // =========================================
        // REGISTER 
        // =========================================
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var exists = await _context.Users
    .AnyAsync(u =>
        u.Email == request.Email &&
        !u.IsDeleted);

            if (exists)
            {
                return new AuthResponse
                {
                    Email = request.Email,
                    FullName = request.Name,
                    Token = "EMAIL_ALREADY_EXISTS"
                };
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(x => x.Name == "Traveller");

            if (role == null)
                throw new Exception("Traveller role not found");

            var user = new User
            {
                FullName = request.Name,
                Email = request.Email,
                Phone = request.Tel,
                PasswordHash = PasswordHasher.HashPassword(request.Password),

                RoleId = role.RoleId,

                IsActive = true,
                IsDeleted = false,

                EmailVerified = false,
                PhoneVerified = false,

                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            //  Generate JWT
            var token = _jwtService.GenerateToken(user, role.Name);

            //  Generate Refresh Token
            user.RefreshToken = Guid.NewGuid().ToString();

            user.RefreshTokenExpiry =
                DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Email = user.Email,
                FullName = user.FullName,
                Token = token,
                RefreshToken = user.RefreshToken
            };
        }

        // =========================================
        // LOGIN
        // =========================================
        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x =>
                    x.Email == request.Email &&
                    !x.IsDeleted);

            if (user == null)
                return null;

            var validPassword = PasswordHasher.VerifyPassword(
                request.Password,
                user.PasswordHash);

            if (!validPassword)
                return null;

            //  Generate JWT
            var token = _jwtService.GenerateToken(
                user,
                user.Role.Name);

            //  Refresh Token
            user.RefreshToken = Guid.NewGuid().ToString();

            user.RefreshTokenExpiry =
                DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Email = user.Email,
                FullName = user.FullName,
                Token = token,
                RefreshToken = user.RefreshToken
            };
        }

        // =========================================
        // REFRESH TOKEN
        // =========================================
        public async Task<string?> RefreshToken(string refreshToken)
        {
            var user = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x =>
                    x.RefreshToken == refreshToken);

            if (user == null)
                return null;

            if (user.RefreshTokenExpiry < DateTime.UtcNow)
                return null;

            var newToken = _jwtService.GenerateToken(
                user,
                user.Role.Name);

            return newToken;
        }

        // =========================================
        // LOGOUT
        // =========================================
        public async Task<bool> Logout(string refreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.RefreshToken == refreshToken);

            if (user == null)
                return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================================
        // FORGOT PASSWORD
        // =========================================
        public async Task<bool> ForgotPassword(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email == email);

            if (user == null)
                return false;

            user.Otp = new Random()
                .Next(100000, 999999)
                .ToString();

            await _context.SaveChangesAsync();

            await _emailService.SendOtpAsync(
                user.Email,
                user.Otp);

            return true;
        }

        // =========================================
        // VERIFY OTP
        // =========================================
        public async Task<bool> VerifyOtp(string email, string otp)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email == email);

            if (user == null)
                return false;

            if (user.Otp != otp)
                return false;

            user.IsVerified = true;

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================================
        // RESET PASSWORD
        // =========================================
        public async Task<bool> ResetPassword(
            string email,
            string otp,
            string newPassword)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email == email);

            if (user == null)
                return false;

            if (user.Otp != otp)
                return false;

            user.PasswordHash =
                PasswordHasher.HashPassword(newPassword);

            user.Otp = null;

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================================
        // DELETE ACCOUNT
        // =========================================
        public async Task<bool> DeleteAccount(int userId)
        {
            var user = await _context.Users
                .FindAsync(userId);

            if (user == null)
                return false;

            user.IsDeleted = true;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}