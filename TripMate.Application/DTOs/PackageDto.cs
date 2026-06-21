public class PackageDto
{
    public DestinationDto Destination { get; set; }

    public List<HotelDto> Hotels { get; set; }

    public List<FlightDto> Flights { get; set; }

    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public string ImageUrl { get; set; } = null!;


}