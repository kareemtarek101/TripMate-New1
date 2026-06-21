using Microsoft.EntityFrameworkCore;
using TripMate.Infrastructure.Persistence;
using TripMate.Infrastructure.Persistence.Entities;

public class DestinationService : IDestinationService
{
    private readonly TripMateDbContext _context;

    public DestinationService(TripMateDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(CreateDestinationRequest request)
    {
        var destination = new Destination
        {
            Name = request.Name,
            CategoryId = request.CategoryId, 
            Country = request.Country,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Price = request.Price,
            DurationDays = request.DurationDays,
            Itinerary = request.Itinerary,
            Activities = request.Activities,
        };

        _context.Destinations.Add(destination);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<DestinationDto?> GetByIdAsync(int id)
    {
        return await _context.Destinations
            .Where(d => d.DestinationId == id)
            .Select(d => new DestinationDto
            {
                Id = d.DestinationId,
                Name = d.Name,
                Country = d.Country,
                Description = d.Description,
                ImageUrl = d.ImageUrl,
                Price = d.Price,
                DurationDays = d.DurationDays,
                Itinerary = d.Itinerary,
                Activities = d.Activities,
                City = d.City,
                AirportCode = d.AirportCode,

                Rating = _context.Ratings
                    .Where(r => r.ItemId == d.DestinationId
                             && r.ItemType == "Destination")
                    .Average(r => (double?)r.Value) ?? 0
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<DestinationDto>> GetAllAsync(int page, int pageSize)
    {
        return await _context.Destinations
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DestinationDto
            {
                Id = d.DestinationId,
                Name = d.Name,
                Country = d.Country,
                Description = d.Description,
                ImageUrl = d.ImageUrl,
                Price = d.Price,
                DurationDays = d.DurationDays,
                Itinerary = d.Itinerary,
                Activities = d.Activities,
                City = d.City,
                AirportCode = d.AirportCode,

                Rating = _context.Ratings
                    .Where(r => r.ItemId == d.DestinationId && r.ItemType == "Destination")
                    .Average(r => (double?)r.Value) ?? 0
            })
            .ToListAsync();
    }


    public async Task<List<DestinationDto>> SearchAsync(string search)
    {
        search = search.ToLower();

        return await _context.Destinations
            .Where(d =>
                d.Name.ToLower().Contains(search) ||
                d.Description.ToLower().Contains(search) ||
                d.Country.ToLower().Contains(search) ||
                d.Activities.ToLower().Contains(search)
            )
            .Select(d => new DestinationDto
            {
                Id = d.DestinationId,
                Name = d.Name,
                Country = d.Country,
                Description = d.Description,
                ImageUrl = d.ImageUrl,
                Price = d.Price,
                DurationDays = d.DurationDays,
                Itinerary = d.Itinerary,
                Activities = d.Activities,
                City = d.City,
                AirportCode = d.AirportCode,

                Rating = _context.Ratings
                    .Where(r => r.ItemId == d.DestinationId && r.ItemType == "Destination")
                    .Average(r => (double?)r.Value) ?? 0
            })
            .ToListAsync();
    }


    public async Task<List<DestinationDto>> FilterAsync(string? country, string? category, decimal? budget)
    {
        var query = _context.Destinations.AsQueryable();

        if (!string.IsNullOrEmpty(country))
            query = query.Where(x => x.Country == country);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(x => x.Category.Name == category);

        if (budget.HasValue)
            query = query.Where(x => x.Price <= budget);

        var result = await query
            .Select(x => new DestinationDto
            {
                Id = x.DestinationId,
                Name = x.Name,
                Price = x.Price
            })
            .ToListAsync();

        return result;
    }
}