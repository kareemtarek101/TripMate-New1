using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

[Index("BookingNumber", Name = "UQ__Bookings__3A30D2BC44476419", IsUnique = true)]
public partial class Booking
{
    [Key]
    [Column("booking_id")]
    public int BookingId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("package_id")]
    public int? PackageId { get; set; }

    [Column("destination_id")]
    public int DestinationId { get; set; }

    [Column("flight_id")]

    public int NumberOfPeople { get; set; }   


    public string Status { get; set; } = "Confirmed";

    public int? FlightId { get; set; }

    [Column("booking_type")]
    [StringLength(30)]
    public string BookingType { get; set; } = null!;

    [Column("currency")]
    [StringLength(10)]
    public string Currency { get; set; } = null!;

    [Column("total_price", TypeName = "decimal(10, 2)")]
    public decimal TotalPrice { get; set; }

    [Column("travel_start_date")]
    public DateOnly TravelStartDate { get; set; }

    [Column("travel_end_date")]
    public DateOnly TravelEndDate { get; set; }

    [Column("status")]
    [StringLength(20)]
    
    public string PaymentStatus { get; set; } = null!;

    [Column("booking_number")]
    [StringLength(50)]
    public string BookingNumber { get; set; } = null!;

    [Column("booked_at")]
    public DateTime BookedAt { get; set; }

    [InverseProperty("Booking")]
    public virtual ICollection<BookingTraveller> BookingTravellers { get; set; } = new List<BookingTraveller>();

    [ForeignKey("DestinationId")]
    [InverseProperty("Bookings")]
    public virtual Destination? Destination { get; set; }

    [ForeignKey("FlightId")]
    [InverseProperty("Bookings")]
    public virtual Flight? Flight { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("Bookings")]
    public virtual Package? Package { get; set; }

    [InverseProperty("Booking")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("UserId")]
    [InverseProperty("Bookings")]
    public virtual User User { get; set; } = null!;
    public DateTime BookingDate { get; set; }
}
