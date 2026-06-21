public class CreatePaymentRequest
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = "User";
    public string LastName { get; set; } = "TripMate";

    public int BookingId { get; set; } 


    public List<BookingItemRequest> Items { get; set; } = new();
}