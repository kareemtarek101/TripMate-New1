using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

[Index("Email", Name = "UQ__Users__AB6E6164DB183792", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public string? Otp { get; set; }
    public bool IsVerified { get; set; } = false;

    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("full_name")]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [Column("email")]
    [StringLength(150)]
    public string Email { get; set; } = null!;

    [Column("phone")]
    [StringLength(30)]
    public string? Phone { get; set; }

    [Column("password_hash")]
    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Column("profile_image_url")]
    [StringLength(255)]
    public string? ProfileImageUrl { get; set; }

    [Column("preferred_language")]
    [StringLength(10)]
    public string? PreferredLanguage { get; set; }

    [Column("preferred_currency")]
    [StringLength(10)]
    
    public string? PreferredCurrency { get; set; }

    [Column("email_verified")]
    public bool EmailVerified { get; set; }

    [Column("phone_verified")]
    public bool PhoneVerified { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("User")]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [InverseProperty("User")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    [InverseProperty("User")]
    public virtual ICollection<RecommendationLog> RecommendationLogs { get; set; } = new List<RecommendationLog>();

    [InverseProperty("User")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<UserAction> UserActions { get; set; } = new List<UserAction>();

    [InverseProperty("User")]
    public virtual UserPreference? UserPreference { get; set; }
    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
