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
    public async Task<List<HotelDto>> GetHotels(string city)
    {
        var apiKey = _config["SerpApi:ApiKey"];

        var url = $"https://serpapi.com/search.json?engine=google_hotels&q={city}&api_key={apiKey}";

        var response = await _httpClient.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();

        var result = new List<HotelDto>();

        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.TryGetProperty("properties", out var hotels))
        {
            foreach (var hotel in hotels.EnumerateArray())
            {
                result.Add(new HotelDto
                {
                    Name = hotel.GetProperty("name").GetString(),
                    Price = hotel.TryGetProperty("rate_per_night", out var price) ? price.ToString() : "N/A",
                    Rating = hotel.TryGetProperty("overall_rating", out var rating) ? rating.GetDouble() : 0
                });
            }
        }

        return result;
    }

    //  Flights
    public async Task<List<FlightDto>> GetFlights(string from, string to)
    {
        var apiKey = _config["SerpApi:ApiKey"];

        var url = $"https://serpapi.com/search.json?engine=google_flights&departure_id={from}&arrival_id={to}&api_key={apiKey}";

        var response = await _httpClient.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();

        var result = new List<FlightDto>();

        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.TryGetProperty("best_flights", out var flights))
        {
            foreach (var flight in flights.EnumerateArray())
            {
                result.Add(new FlightDto
                {
                    Airline = flight.TryGetProperty("airline", out var airline) ? airline.GetString() : "N/A",
                    From = from,
                    To = to,
                    Price = flight.TryGetProperty("price", out var price) ? price.ToString() : "N/A",
                    Duration = flight.TryGetProperty("duration", out var duration) ? duration.ToString() : "N/A"
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

        var hotels = await GetHotels(to);
        var flights = await GetFlights(from, to);

        return new PackageDto
        {
            Destination = destination,
            Hotels = hotels,
            Flights = flights
        };
    }
}