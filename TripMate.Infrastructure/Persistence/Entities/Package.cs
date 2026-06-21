using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMate.Infrastructure.Persistence.Entities;

public class Package
{
    [Key]
    [Column("package_id")]
    public int PackageId { get; set; }

    [Column("destination_id")]
    public int DestinationId { get; set; }

    [Column("start_date")]
    public DateOnly StartDate { get; set; }

    [Column("end_date")]
    public DateOnly EndDate { get; set; }

    [Column("max_guests")]
    public int MaxGuests { get; set; }

    // 🔥 خليه Name عشان consistency
    [Column("name")]
    [StringLength(150)]
    public string Name { get; set; } = null!;

    [Column("short_description")]
    [StringLength(255)]
    public string? ShortDescription { get; set; }

    [Column("full_description")]
    public string? FullDescription { get; set; }

    [Column("duration_days")]
    public int DurationDays { get; set; }

    [Column("base_price", TypeName = "decimal(10, 2)")]
    public decimal BasePrice { get; set; }

    [Column("currency")]
    [StringLength(10)]
    public string Currency { get; set; } = "USD";

    // 🔥 مهم للـ UI (الكارت)
    [Column("image_url")]
    public string? ImageUrl { get; set; }

    [Column("available_from")]
    public DateOnly? AvailableFrom { get; set; }

    [Column("available_to")]
    public DateOnly? AvailableTo { get; set; }

    [Column("included_text")]
    public string? IncludedText { get; set; }

    [Column("excluded_text")]
    public string? ExcludedText { get; set; }

    [Column("average_rating", TypeName = "decimal(3, 2)")]
    public decimal? AverageRating { get; set; }

    [Column("tags")]
    [StringLength(255)]
    public string? Tags { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // 🔗 Navigation (سيبهم عادي)
    [ForeignKey("DestinationId")]
    [InverseProperty("Packages")]
    public virtual Destination Destination { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<PackageFlight> PackageFlights { get; set; } = new List<PackageFlight>();

    public virtual ICollection<PackageMedium> PackageMedia { get; set; } = new List<PackageMedium>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<RecommendationLog> RecommendationLogs { get; set; } = new List<RecommendationLog>();

    public virtual ICollection<UserAction> UserActions { get; set; } = new List<UserAction>();
}