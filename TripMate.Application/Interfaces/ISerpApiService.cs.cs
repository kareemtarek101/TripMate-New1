public interface ISerpApiService
{
    Task<List<HotelDto>> GetHotels(
        string city,
        DateOnly checkIn,
        DateOnly checkOut
    );
    Task<List<FlightDto>> GetFlights(
        string from,
        string to,
        DateOnly outboundDate
    );
    Task<PackageDto> GetPackage(string from, string to);

}