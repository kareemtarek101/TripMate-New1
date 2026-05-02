public interface ISerpApiService
{
    Task<List<HotelDto>> GetHotels(string city);

    Task<List<FlightDto>> GetFlights(string from, string to);

    Task<PackageDto> GetPackage(string from, string to);

}