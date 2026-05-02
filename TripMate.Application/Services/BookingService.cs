using Microsoft.EntityFrameworkCore;
using TripMate.Infrastructure.Persistence;
using TripMate.Infrastructure.Persistence.Entities;

public class BookingService : IBookingService
{
    private readonly TripMateDbContext _context;

    public BookingService(TripMateDbContext context)
    {
        _context = context;
    }

    // ✅ إنشاء حجز
    public async Task<bool> CreateBooking(int userId, CreateBookingRequest request)
    {
        // 🔥 تأكد إن الرحلة موجودة
        var destinationExists = await _context.Destinations
            .AnyAsync(d => d.DestinationId == request.DestinationId);

        if (!destinationExists)
            return false;

        var booking = new Booking
        {
            UserId = userId,
            DestinationId = request.DestinationId,
            BookingDate = DateTime.UtcNow,
            NumberOfPeople = request.NumberOfPeople,
            Status = "Confirmed"
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return true;
    }

    // ✅ كل حجوزات اليوزر
    public async Task<List<BookingDto>> GetUserBookings(int userId)
    {
        return await _context.Bookings
            .Where(x => x.UserId == userId)
            .Select(x => new BookingDto
            {
                Id = x.BookingId,
                DestinationId = x.DestinationId,
                BookingDate = x.BookingDate,
                NumberOfPeople = x.NumberOfPeople,
                Status = x.Status
            })
            .ToListAsync();
    }

    // ✅ حجز واحد
    public async Task<Booking?> GetBookingById(int id)
    {
        return await _context.Bookings
            .FirstOrDefaultAsync(x => x.BookingId == id);
    }

    // ✅ إلغاء الحجز (Soft Cancel)
    public async Task<bool> CancelBooking(int bookingId, int userId)
    {
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(x =>
                x.BookingId == bookingId &&
                x.UserId == userId);

        if (booking == null)
            return false;

        booking.Status = "Cancelled";

        await _context.SaveChangesAsync();

        return true;
    }
}