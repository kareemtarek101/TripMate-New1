using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TripMate.Application.Auth.Dtos;
using TripMate.Application.Auth.Interfaces;
using TripMate.Infrastructure.Persistence;
using TripMate.Infrastructure.Persistence.Entities;
using TripMate.Infrastructure.Security;

namespace TripMate.Application.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly TripMateDbContext _context;

        public AuthService(TripMateDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ResetPassword(string email, string otp, string newPassword)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return false;

            if (user.Otp != otp)
                return false;

            user.PasswordHash = PasswordHasher.HashPassword(newPassword);

            user.Otp = null;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAccount(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return false;

            user.IsDeleted = true;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> ForgotPassword(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return false;

            user.Otp = new Random().Next(100000, 999999).ToString();

            await _context.SaveChangesAsync();

            Console.WriteLine($"OTP: {user.Otp}");

            return true;
        }
        public async Task<string?> RefreshToken(string refreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if (user == null) return null;

            if (user.RefreshTokenExpiry < DateTime.UtcNow)
                return null;

            var newToken = GenerateJwtToken(user);

            return newToken;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
              new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
              new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("YOUR_SECRET_KEY")
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> Logout(string refreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if (user == null) return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
             .FirstOrDefaultAsync(u =>
                   u.Email == request.Email &&
                   !u.IsDeleted);

            if (user == null)
                return null;

            if (!PasswordHasher.VerifyPassword(
                request.Password,
                user.PasswordHash))
                return null;

            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Email = user.Email,
                FullName = user.FullName,
                Token = token,
                RefreshToken = user.RefreshToken
            };

        }
        public async Task<bool> VerifyOtp(string email, string otp)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null) return false;

            if (user.Otp != otp) return false;

            user.IsVerified = true;
            user.Otp = null;

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var exists = await _context.Users
                .AnyAsync(u => u.Email == request.Email);



            if (exists)
                throw new Exception("Email already exists");

            var role = await  _context.Roles.FirstOrDefaultAsync(x => x.Name == "Traveller");
            if (role is null)
            {
                throw new Exception("Role doesn't exists");

            }

            var user = new Infrastructure.Persistence.Entities.User
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                RoleId = role.RoleId,//***
                IsActive = true,
                EmailVerified = false,
                PhoneVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Email = user.Email,
                FullName = user.FullName,
                Token = "TEMP_TOKEN"
            };
        }
    }
}
