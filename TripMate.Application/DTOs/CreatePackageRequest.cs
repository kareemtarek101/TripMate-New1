public class CreatePackageRequest
{
    public int DestinationId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public string ImageUrl { get; set; } = null!;
    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int MaxGuests { get; set; }
}