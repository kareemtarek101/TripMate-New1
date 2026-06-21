using TripMate.Infrastructure.Persistence.Entities;

public class BookingItem
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public string ItemType { get; set; } = null!;
    // Package / Hotel / Flight

    public int ItemId { get; set; }

    public decimal Price { get; set; }

    public Booking Booking { get; set; } = null!;
}