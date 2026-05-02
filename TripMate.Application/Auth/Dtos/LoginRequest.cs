using System.ComponentModel.DataAnnotations;

namespace TripMate.Application.Auth.Dtos
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}

