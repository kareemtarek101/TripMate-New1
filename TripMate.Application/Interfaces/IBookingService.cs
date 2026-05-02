using TripMate.Application;
using TripMate.Infrastructure.Persistence.Entities;

public interface IBookingService
{

    Task<List<BookingDto>> GetUserBookings(int userId);

    Task<Booking?> GetBookingById(int id);

    Task<bool> CreateBooking(int userId, CreateBookingRequest request);
    Task<bool> CancelBooking(int bookingId, int userId);
}