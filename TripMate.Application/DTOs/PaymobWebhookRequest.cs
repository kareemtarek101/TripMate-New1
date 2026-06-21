using System.Text.Json.Serialization;

public class PaymobWebhookRequest
{
    public TransactionObj Obj { get; set; } = null!;
}

public class TransactionObj
{
    public bool Success { get; set; }
    public int Id { get; set; }
    public OrderObj Order { get; set; } = null!;

    [JsonPropertyName("amount_cents")]
    public int AmountCents { get; set; }
}

public class OrderObj
{
    public int Id { get; set; }

    [JsonPropertyName("merchant_order_id")]
    public string? MerchantOrderId { get; set; }
}