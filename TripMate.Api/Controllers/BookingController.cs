using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripMate.Infrastructure.Persistence.Entities;

namespace TripMate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IInteractionService _interactionService;

        public BookingsController(
            IBookingService bookingService,
            IInteractionService interactionService)
        {
            _bookingService = bookingService;
            _interactionService = interactionService;
        }

        // 🔍 تفاصيل حجز
        [Authorize]
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var booking = await _bookingService.GetBookingById(id);

            if (booking == null)
                return NotFound(new ApiResponse<string>("Booking not found"));

            return Ok(new ApiResponse<Booking>(booking));
        }

        // 🔥 إنشاء حجز + تسجيل interaction
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _bookingService.CreateBooking(userId, request);

            if (!result)
                return BadRequest(new ApiResponse<string>("Invalid destination"));

            // 🔥 أهم سطر
            await _interactionService.AddInteraction(userId, request.DestinationId, "Booking");

            return Ok(new ApiResponse<string>("Booking created"));
        }

        // 📋 حجوزاتي
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _bookingService.GetUserBookings(userId);

            return Ok(new ApiResponse<List<BookingDto>>(result));
        }

        // ❌ إلغاء حجز
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _bookingService.CancelBooking(id, userId);

            if (!result)
                return BadRequest(new ApiResponse<string>("Booking not found"));

            return Ok(new ApiResponse<string>("Booking cancelled"));
        }
    }
}