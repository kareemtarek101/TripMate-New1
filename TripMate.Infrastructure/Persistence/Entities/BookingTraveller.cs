using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

public partial class BookingTraveller
{
    [Key]
    [Column("traveler_id")]
    public int TravelerId { get; set; }

    [Column("booking_id")]
    public int BookingId { get; set; }

    [Column("full_name")]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [Column("date_of_birth")]
    public DateOnly? DateOfBirth { get; set; }

    [Column("passport_number")]
    [StringLength(50)]
    public string? PassportNumber { get; set; }

    [Column("nationality")]
    [StringLength(50)]
    public string? Nationality { get; set; }

    [Column("contact_phone")]
    [StringLength(30)]
    public string? ContactPhone { get; set; }

    [ForeignKey("BookingId")]
    [InverseProperty("BookingTravellers")]
    public virtual Booking Booking { get; set; } = null!;
}
