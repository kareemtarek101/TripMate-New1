using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;
using TripMate.Infrastructure.Persistence;
using TripMate.Infrastructure.Persistence.Entities;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(
        IPaymentService paymentService,
        ILogger<PaymentsController> logger,
        TripMateDbContext context)
    {
        _paymentService = paymentService;
        _logger = logger;
        _context = context;
    }

    // =========================
    // CREATE PAYMENT
    // =========================
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Pay([FromBody] CreatePaymentRequest request)
    {
        try
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            request.UserId = userId;

            var url = await _paymentService.CreatePaymentAsync(request);

            return Ok(new
            {
                paymentUrl = url
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment");

            return StatusCode(500, new
            {
                message = "Payment failed",
                error = ex.Message
            });
        }
    }

    // =========================
    // PAYMOB CALLBACK
    // =========================
    [AllowAnonymous]
    [HttpPost("callback")]
    public async Task<IActionResult> Callback([FromBody] PaymobWebhookRequest request)
    {
        try
        {
            _logger.LogInformation("========== PAYMOB CALLBACK ==========");
            _logger.LogInformation(JsonSerializer.Serialize(request));

            if (request == null || request.Obj == null)
            {
                _logger.LogWarning("Callback body is null");
                return BadRequest("Invalid payload");
            }

            _logger.LogInformation(
                $"TransactionId = {request.Obj.Id}");

            _logger.LogInformation(
                $"Success = {request.Obj.Success}");

            _logger.LogInformation(
                $"MerchantOrderId = {request.Obj.Order?.MerchantOrderId}");

            if (request.Obj.Success)
            {
                await _paymentService.HandlePaymentSuccessAsync(request);

                _logger.LogInformation(
                    $"Payment Success handled for Booking = {request.Obj.Order?.MerchantOrderId}");
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Paymob Callback Error");

            return BadRequest(new
            {
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    private readonly TripMateDbContext _context;
    [HttpPost("confirm-payment")]

    public async Task<IActionResult> ConfirmPayment(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);

        if (booking == null)
            return NotFound("Booking not found");

        booking.PaymentStatus = "Paid";
        booking.Status = "Confirmed";

        _context.Payments.Add(new Payment
        {
            BookingId = booking.BookingId,
            Amount = booking.TotalPrice,
            Currency = "EGP",
            GatewayType = "Paymob",
            TransactionStatus = "Success",
            TransactionReference = Guid.NewGuid().ToString(),
            PaidAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return Ok("Payment Confirmed");
    }

}