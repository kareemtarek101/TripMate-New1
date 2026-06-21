using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMate.Infrastructure.Persistence.Entities;

[Index("BookingNumber", Name = "UQ__Bookings__3A30D2BC44476419", IsUnique = true)]
public class Booking
{
    [Key]
    [Column("booking_id")]
    public int BookingId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    // 🔥 سيبهم optional (backward compatibility)
    [Column("package_id")]
    public int? PackageId { get; set; }

    [Column("destination_id")]
    public int? DestinationId { get; set; }

    [Column("flight_id")]
    public int? FlightId { get; set; }

    // 👥 عدد الأشخاص
    [Column("number_of_people")]
    public int NumberOfPeople { get; set; }

    // 🔥 حالة الحجز
    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = "Pending";

    // 💰 حالة الدفع
    [Column("payment_status")]
    [StringLength(20)]
    public string PaymentStatus { get; set; } = "Pending";

    [Column("booking_type")]
    [StringLength(30)]
    public string? BookingType { get; set; }

    [Column("currency")]
    [StringLength(10)]
    public string Currency { get; set; } = "EGP";

    [Column("total_price", TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    [Column("travel_start_date")]
    public DateOnly? TravelStartDate { get; set; }

    [Column("travel_end_date")]
    public DateOnly? TravelEndDate { get; set; }

    [Column("booking_number")]
    [StringLength(50)]
    public string BookingNumber { get; set; } = Guid.NewGuid().ToString();

    [Column("booked_at")]
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;

    // 🔥 NEW (المهم)
    public ICollection<BookingItem> Items { get; set; } = new List<BookingItem>();

    // 🔗 Relations
    public virtual User User { get; set; } = null!;

    public virtual Destination? Destination { get; set; }

    public virtual Flight? Flight { get; set; }

    public virtual Package? Package { get; set; }

    public virtual ICollection<BookingTraveller> BookingTravellers { get; set; } = new List<BookingTraveller>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}