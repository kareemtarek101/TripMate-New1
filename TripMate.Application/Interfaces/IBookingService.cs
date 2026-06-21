using TripMate.Application;
using TripMate.Infrastructure.Persistence.Entities;

public interface IBookingService
{


    Task<BookingDto?> GetBookingById(int id);
    Task<bool> CreateBooking(int userId, CreateBookingRequest request);
    Task<bool> CancelBooking(int bookingId, int userId);
    Task<List<BookingDto>> GetUserBookingsAsync(int userId);
}