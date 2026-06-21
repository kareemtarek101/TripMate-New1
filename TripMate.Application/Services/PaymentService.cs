using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TripMate.Infrastructure.Persistence;
using TripMate.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Http;

public class PaymentService : IPaymentService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly TripMateDbContext _context;
    private readonly IHttpContextAccessor _httpContext;

    public PaymentService(
        HttpClient http,
        IConfiguration config,
        TripMateDbContext context,
        IHttpContextAccessor httpContext)
    {
        _http = http;
        _config = config;
        _context = context;
        _httpContext = httpContext;
    }

    // =========================
    // 💳 CREATE PAYMENT
    // =========================
    public async Task<string> CreatePaymentAsync(CreatePaymentRequest request)
    {
        Console.WriteLine(
    $"BOOKING ID = {request.BookingId}"
);
        var apiKey = _config["Paymob:ApiKey"];
        var integrationId = int.Parse(_config["Paymob:IntegrationId"]);
        var iframeId = _config["Paymob:IframeId"];

        if (string.IsNullOrEmpty(apiKey))
            throw new Exception("Paymob API Key is missing");

        var authToken = await GetAuthToken(apiKey);

        // 🔥 مهم جدًا: نبعت bookingId
        var orderId = await CreateOrder(authToken, request.Amount, request.BookingId);

        // 💾 save transaction
        _context.PaymentTransactions.Add(new PaymentTransaction
        {
            UserId = request.UserId,
            Amount = request.Amount,
            OrderId = orderId.ToString(),
            IsPaid = false,
            ItemsJson = JsonSerializer.Serialize(request.Items),
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        var paymentKey = await CreatePaymentKey(authToken, orderId, request, integrationId);

        return $"https://accept.paymob.com/api/acceptance/iframes/{iframeId}?payment_token={paymentKey}";
    }

    // =========================
    // 🔐 AUTH
    // =========================
    private async Task<string> GetAuthToken(string apiKey)
    {
        var response = await _http.PostAsync(
            "https://accept.paymob.com/api/auth/tokens",
            new StringContent(JsonSerializer.Serialize(new { api_key = apiKey }), Encoding.UTF8, "application/json"));

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth failed: {content}");

        return JsonDocument.Parse(content).RootElement.GetProperty("token").GetString()!;
    }

    // =========================
    // 🛒 CREATE ORDER
    // =========================
    private async Task<int> CreateOrder(string authToken, decimal amount, int bookingId)
    {
        var response = await _http.PostAsync(
            "https://accept.paymob.com/api/ecommerce/orders",
            new StringContent(JsonSerializer.Serialize(new
            {
                auth_token = authToken,
                delivery_needed = false,
                amount_cents = (int)(amount * 100),
                currency = "EGP",
                merchant_order_id = bookingId.ToString(), // 🔥 أهم سطر
                items = new object[] { }
            }), Encoding.UTF8, "application/json"));

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Order creation failed: {content}");

        return JsonDocument.Parse(content).RootElement.GetProperty("id").GetInt32();
    }

    // =========================
    // 💳 PAYMENT KEY
    // =========================
    private async Task<string> CreatePaymentKey(
        string authToken,
        int orderId,
        CreatePaymentRequest request,
        int integrationId)
    {
        var response = await _http.PostAsync(
            "https://accept.paymob.com/api/acceptance/payment_keys",
            new StringContent(JsonSerializer.Serialize(new
            {
                auth_token = authToken,
                amount_cents = (int)(request.Amount * 100),
                expiration = 3600,
                order_id = orderId,
                billing_data = new
                {
                    email = request.Email,
                    first_name = request.FirstName ?? "User",
                    last_name = request.LastName ?? "TripMate",
                    phone_number = "01000000000",
                    street = "NA",
                    building = "NA",
                    floor = "NA",
                    apartment = "NA",
                    city = "Cairo",
                    country = "EG",
                    postal_code = "12345"
                },
                currency = "EGP",
                integration_id = integrationId
            }), Encoding.UTF8, "application/json"));

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Payment key failed: {content}");

        return JsonDocument.Parse(content).RootElement.GetProperty("token").GetString()!;
    }

    // =========================
    // 🔐 HMAC
    // =========================
    public bool ValidateHmac(PaymobWebhookRequest request)
    {
        if (request?.Obj == null)
            return false;

        var hmacSecret = _config["Paymob:HmacSecret"];

        if (string.IsNullOrEmpty(hmacSecret))
            return false;

        var data = $"{request.Obj.AmountCents}" +
                   $"{request.Obj.Id}" +
                   $"{request.Obj.Order.Id}" +
                   $"{request.Obj.Success}";

        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(hmacSecret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        var generated = BitConverter.ToString(hash).Replace("-", "").ToLower();

        var received = _httpContext.HttpContext?.Request.Query["hmac"].ToString();

        if (string.IsNullOrEmpty(received))
            return false;

        return generated == received;
    }

    // =========================
    // 🔥 SUCCESS
    // =========================
    public async Task HandlePaymentSuccessAsync(PaymobWebhookRequest request)
    {
        var transactionId = request.Obj.Id.ToString();
        var amount = request.Obj.AmountCents / 100m;

        var exists = await _context.Payments
            .AnyAsync(p => p.TransactionReference == transactionId);

        if (exists)
            return;

        var bookingIdString = request.Obj.Order.MerchantOrderId;

        if (!int.TryParse(bookingIdString, out var bookingId))
            throw new Exception("Invalid BookingId");

        var booking = await _context.Bookings.FindAsync(bookingId);

        if (booking == null)
            throw new Exception("Booking not found");

        using var trx = await _context.Database.BeginTransactionAsync();

        try
        {
            booking.PaymentStatus = "Paid";
            booking.Status = "Confirmed";

            _context.Payments.Add(new Payment
            {
                BookingId = bookingId,
                Amount = amount,
                TransactionStatus = "Success",
                TransactionReference = transactionId,
                Currency = "EGP",
                GatewayType = "Paymob",
                PaidAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            await trx.CommitAsync();
        }
        catch
        {
            await trx.RollbackAsync();
            throw;
        }
    }
}