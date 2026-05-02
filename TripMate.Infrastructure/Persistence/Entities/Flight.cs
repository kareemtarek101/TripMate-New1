using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

public partial class Flight
{
    [Key]
    [Column("flight_id")]
    public int FlightId { get; set; }

    [Column("origin_airport")]
    [StringLength(100)]
    public string OriginAirport { get; set; } = null!;

    [Column("destination_airport")]
    [StringLength(100)]
    public string DestinationAirport { get; set; } = null!;

    [Column("airline_name")]
    [StringLength(100)]
    public string AirlineName { get; set; } = null!;

    [Column("flight_number")]
    [StringLength(20)]
    public string FlightNumber { get; set; } = null!;

    [Column("departure_datetime")]
    public DateTime DepartureDatetime { get; set; }

    [Column("arrival_datetime")]
    public DateTime ArrivalDatetime { get; set; }

    [Column("duration_minutes")]
    public int DurationMinutes { get; set; }

    [Column("price", TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [Column("currency")]
    [StringLength(10)]
    public string Currency { get; set; } = null!;

    [Column("cabin_class")]
    [StringLength(20)]
    public string? CabinClass { get; set; }

    [Column("seats_available")]
    public int? SeatsAvailable { get; set; }

    [Column("baggage_policy")]
    [StringLength(255)]
    public string? BaggagePolicy { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Flight")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("Flight")]
    public virtual ICollection<PackageFlight> PackageFlights { get; set; } = new List<PackageFlight>();

    [InverseProperty("Flight")]
    public virtual ICollection<RecommendationLog> RecommendationLogs { get; set; } = new List<RecommendationLog>();

    [InverseProperty("Flight")]
    public virtual ICollection<UserAction> UserActions { get; set; } = new List<UserAction>();
}
