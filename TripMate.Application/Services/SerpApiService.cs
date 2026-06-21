using Microsoft.Extensions.Configuration;
using System.Text.Json;
using TripMate.Application;

public class SerpApiService : ISerpApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly IDestinationService _destinationService;

    public SerpApiService(
        HttpClient httpClient,
        IConfiguration config,
        IDestinationService destinationService)
    {
        _httpClient = httpClient;
        _config = config;
        _destinationService = destinationService;
    }

    //  Hotels
    public async Task<List<HotelDto>> GetHotels(
        string city,
        DateOnly checkIn,
        DateOnly checkOut)
    {
        var apiKey = _config["SerpApi:ApiKey"];

        var url =
$"https://serpapi.com/search.json" +
$"?engine=google_hotels" +
$"&q={city}" +
$"&check_in_date={checkIn:yyyy-MM-dd}" +
$"&check_out_date={checkOut:yyyy-MM-dd}" +
$"&api_key={apiKey}";

        var response = await _httpClient.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        System.Diagnostics.Debug.WriteLine("HOTELS RESPONSE:");
        System.Diagnostics.Debug.WriteLine(json);
        Console.WriteLine("HOTELS RESPONSE:");
        Console.WriteLine(json);


        var result = new List<HotelDto>();

        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.TryGetProperty("properties", out var hotels))
        {
            foreach (var hotel in hotels.EnumerateArray())
            {
                result.Add(new HotelDto
                {
                    Name = hotel.GetProperty("name").GetString(),
                    Price =
hotel.TryGetProperty("rate_per_night", out var price)
&& price.TryGetProperty("lowest", out var lowest)
    ? lowest.GetString()
    : "N/A",
                    Rating = hotel.TryGetProperty("overall_rating", out var rating) ? rating.GetDouble() : 0
                });
            }
        }

        return result;
    }

    //  Flights
    public async Task<List<FlightDto>> GetFlights(
        string from,
        string to,
        DateOnly outboundDate)
    {
        var apiKey = _config["SerpApi:ApiKey"];

        var returnDate = outboundDate.AddDays(5);

        var url =
        $"https://serpapi.com/search.json" +
        $"?engine=google_flights" +
        $"&departure_id={from}" +
        $"&arrival_id={to}" +
        $"&outbound_date={outboundDate:yyyy-MM-dd}" +
        $"&return_date={returnDate:yyyy-MM-dd}" +
        $"&type=1" +
        $"&api_key={apiKey}";
        var response = await _httpClient.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        System.Diagnostics.Debug.WriteLine("FLIGHTS RESPONSE:");
        System.Diagnostics.Debug.WriteLine(json);
        Console.WriteLine("FLIGHTS RESPONSE:");
        Console.WriteLine(json);

        var result = new List<FlightDto>();

        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.TryGetProperty("best_flights", out var flights))
        {
            foreach (var flight in flights.EnumerateArray())
            {
                string airlineName = "N/A";

                if (flight.TryGetProperty("flights", out var segments)
                    && segments.GetArrayLength() > 0)
                {
                    airlineName = segments[0]
                        .GetProperty("airline")
                        .GetString() ?? "N/A";
                }

                result.Add(new FlightDto
                {
                    Airline = airlineName,

                    From = from,

                    To = to,

                    Price = flight.TryGetProperty("price", out var price)
                        ? price.ToString()
                        : "N/A",

                    Duration = flight.TryGetProperty("total_duration", out var duration)
                        ? $"{duration.GetInt32()} min"
                        : "N/A"
                });
            }
        }

        return result;
    }

    //  Package (Destination + Hotels + Flights)
    public async Task<PackageDto> GetPackage(string from, string to)
    {
        //  هات destination من السيستم عندك
        var destinations = await _destinationService.SearchAsync(to);
        var destination = destinations.FirstOrDefault();

        //  fallback لو مفيش
        if (destination == null)
        {
            destination = (await _destinationService.GetAllAsync(1, 1)).FirstOrDefault();
        }

        var hotels = await GetHotels(
            to,
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(12))
        );
        var flights = await GetFlights(
    from,
    to,
    DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7))
);

        return new PackageDto
        {
            Destination = destination,
            Hotels = hotels,
            Flights = flights
        };
    }
}