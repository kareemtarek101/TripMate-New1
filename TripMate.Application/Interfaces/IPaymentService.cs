public interface IPaymentService
{
    Task<string> CreatePaymentAsync(CreatePaymentRequest request);
    bool ValidateHmac(PaymobWebhookRequest request);
    Task HandlePaymentSuccessAsync(PaymobWebhookRequest request);
}