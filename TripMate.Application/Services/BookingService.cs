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
        var destination = await _context.Destinations
    .FirstOrDefaultAsync(d => d.DestinationId == request.DestinationId);

        if (destination == null)
            return false;

        var booking = new Booking
        {
            UserId = userId,
            DestinationId = request.DestinationId,

            BookedAt = DateTime.UtcNow,

            NumberOfPeople = request.NumberOfPeople,

            TotalPrice = destination.Price * request.NumberOfPeople,

            Currency = "EGP",

            PaymentStatus = "Pending",

            Status = "Confirmed",

            BookingType = "Destination"
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return true;
    }

    // ✅ كل حجوزات اليوزر
    public async Task<List<BookingDto>> GetUserBookingsAsync(int userId)
    {
        var bookings = await _context.Bookings
            .Where(b => b.UserId == userId)
            .Include(b => b.Destination)
            .Include(b => b.Package)
            .OrderByDescending(b => b.BookedAt)
            .Select(b => new BookingDto
            {
                Id = b.BookingId,

                UserId = b.UserId,
                DestinationId = b.DestinationId ?? 0,
                NumberOfPeople = b.NumberOfPeople,

                BookingNumber = b.BookingNumber,
                BookingType = b.BookingType,
                TotalPrice = b.TotalPrice,
                Currency = b.Currency,
                PaymentStatus = b.PaymentStatus,
                Status = b.Status,
                BookingDate = b.BookedAt,

                DestinationName = b.Destination != null ? b.Destination.Name : null,
                PackageName = b.Package != null ? b.Package.Name : null
            })
            .ToListAsync();

        return bookings;
    }

    // ✅ حجز واحد
    public async Task<BookingDto?> GetBookingById(int id)
    {
        return await _context.Bookings
            .Include(b => b.Destination)
            .Include(b => b.Package)
            .Where(b => b.BookingId == id)
            .Select(b => new BookingDto
            {
                Id = b.BookingId,

                UserId = b.UserId,
                DestinationId = b.DestinationId ?? 0,
                NumberOfPeople = b.NumberOfPeople,

                BookingNumber = b.BookingNumber,
                BookingType = b.BookingType,
                TotalPrice = b.TotalPrice,
                Currency = b.Currency,
                PaymentStatus = b.PaymentStatus,
                Status = b.Status,
                BookingDate = b.BookedAt,

                DestinationName = b.Destination != null
                    ? b.Destination.Name
                    : null,

                PackageName = b.Package != null
                    ? b.Package.Name
                    : null
            })
            .FirstOrDefaultAsync();
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