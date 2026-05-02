using System.ComponentModel.DataAnnotations;

namespace TripMate.Application.Auth.Dtos
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]

        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        public string? Phone { get; set; }
    }
}
