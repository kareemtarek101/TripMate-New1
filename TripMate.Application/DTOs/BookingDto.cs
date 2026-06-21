public class BookingDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int DestinationId { get; set; }
    public DateTime BookingDate { get; set; }
    public int NumberOfPeople { get; set; }

    public string Status { get; set; } = "Confirmed";

    public string BookingNumber { get; set; } = null!;
    public string BookingType { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;

    //  Info للـ UI
    public string? DestinationName { get; set; }
    public string? PackageName { get; set; }


}