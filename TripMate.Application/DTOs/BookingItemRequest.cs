public class BookingItemRequest
{
    public string ItemType { get; set; } = null!;
    public int ItemId { get; set; }
    public decimal Price { get; set; }
}