using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

public partial class Destination
{
    [Key]
    [Column("destination_id")]
    public int DestinationId { get; set; }

    [Column("trip_type_id")]
    public int? TripTypeId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("country")]
    [StringLength(100)]
    public string Country { get; set; } = null!;

    [Column("city")]
    [StringLength(100)]

    public string ImageUrl { get; set; }
    public string? City { get; set; }

    public decimal Price { get; set; }

    public int DurationDays { get; set; }

    public string Itinerary { get; set; } // برنامج الرحلة

    public string? Activities { get; set; } // نص بسيط مؤقت

    [Column("description")]
    public string? Description { get; set; }

    [Column("main_image_url")]
    [StringLength(255)]
    public string? MainImageUrl { get; set; }

    [Column("average_rating", TypeName = "decimal(3, 2)")]
    public decimal? AverageRating { get; set; }

    [Column("min_price_estimate", TypeName = "decimal(10, 2)")]
    public decimal? MinPriceEstimate { get; set; }

    [Column("timezone")]
    [StringLength(50)]
    public string? Timezone { get; set; }

    [Column("latitude", TypeName = "decimal(9, 6)")]
    public decimal? Latitude { get; set; }

    [Column("longitude", TypeName = "decimal(9, 6)")]
    public decimal? Longitude { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Destination")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("Destination")]
    public virtual ICollection<DestinationMedium> DestinationMedia { get; set; } = new List<DestinationMedium>();


    [InverseProperty("Destination")]
    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();

    [InverseProperty("Destination")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    [InverseProperty("Destination")]
    public virtual ICollection<RecommendationLog> RecommendationLogs { get; set; } = new List<RecommendationLog>();

    [ForeignKey("TripTypeId")]
    [InverseProperty("Destinations")]
    public virtual TripType? TripType { get; set; }

    [InverseProperty("Destination")]
    public virtual ICollection<UserAction> UserActions { get; set; } = new List<UserAction>();

    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
