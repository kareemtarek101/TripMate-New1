public class PaymentTransaction
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string OrderId { get; set; } = null!;

    public bool IsPaid { get; set; }

    public string ItemsJson { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}