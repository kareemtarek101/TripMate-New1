using Microsoft.EntityFrameworkCore;
using TripMate.Application;
using TripMate.Infrastructure.Persistence;
using TripMate.Infrastructure.Persistence.Entities;

public class InteractionService : IInteractionService
{
    private readonly TripMateDbContext _context;

    public InteractionService(TripMateDbContext context)
    {
        _context = context;
    }

    //  تسجيل Interaction (view / search / click)
    public async Task AddInteraction(int userId, int destinationId, string type)
    {
        var interaction = new UserInteraction
        {
            UserId = userId,
            DestinationId = destinationId,
            InteractionType = type,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserInteractions.Add(interaction);
        await _context.SaveChangesAsync();
    }

    //  Recommendation System (Super Hybrid)
    public async Task<List<DestinationDto>> GetSuperRecommendations(int userId, decimal budget)
    {
        //  Interactions
        var interactions = await _context.UserInteractions
            .Where(x => x.UserId == userId)
            .Select(x => x.DestinationId)
            .ToListAsync();

        //  Favorites
        var favorites = await _context.Favorites
            .Where(x => x.UserId == userId && x.ItemType == "Destination")
            .Select(x => x.ItemId)
            .ToListAsync();

        //  Bookings
        var bookings = await _context.Bookings
            .Where(x => x.UserId == userId)
            .Select(x => x.DestinationId)
            .ToListAsync();

        var data = await _context.Destinations
            .Select(d => new
            {
                Destination = d,

                //  Rating
                Rating = _context.Ratings
                    .Where(r => r.ItemId == d.DestinationId && r.ItemType == "Destination")
                    .Average(r => (double?)r.Value) ?? 0,

                //  Budget match
                BudgetScore = 1 - Math.Abs((double)(d.Price - budget) / (double)budget),

                //  Interaction
                InteractionScore = interactions.Contains(d.DestinationId) ? 1 : 0,

                //  Favorite
                FavoriteScore = favorites.Contains(d.DestinationId) ? 1 : 0,

                //  Booking
                BookingScore = bookings.Contains(d.DestinationId) ? 1 : 0
            })
            .ToListAsync();

        var result = data
            .Select(x => new
            {
                x.Destination,
                x.Rating,
                Score =
                    (x.InteractionScore * 0.25) +
                    (x.FavoriteScore * 0.25) +
                    (x.BookingScore * 0.2) +
                    (x.BudgetScore * 0.15) +
                    (x.Rating * 0.15)
            })
            .OrderByDescending(x => x.Score)
            .Take(10)
            .Select(x => new DestinationDto
            {
                Id = x.Destination.DestinationId,
                Name = x.Destination.Name,
                Country = x.Destination.Country,
                Description = x.Destination.Description,
                ImageUrl = x.Destination.ImageUrl,
                Price = x.Destination.Price,
                DurationDays = x.Destination.DurationDays,
                Itinerary = x.Destination.Itinerary,
                Activities = x.Destination.Activities,
                Rating = x.Rating
            })
            .ToList();

        return result;
    }
}