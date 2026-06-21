using Microsoft.EntityFrameworkCore;
using TripMate.Application;
using TripMate.Infrastructure.Persistence;
using TripMate.Infrastructure.Persistence.Entities;

public class PackageService : IPackageService
{
    private readonly TripMateDbContext _context;

    public PackageService(TripMateDbContext context)
    {
        _context = context;
    }

    // GET ALL
    public async Task<List<PackageDto>> GetAllAsync()
    {
        return await _context.Packages
            .Where(p => p.IsActive)
            .Select(p => new PackageDto
            {
                Id = p.PackageId,
                Title = p.Name,               //  mapping
                Price = p.BasePrice,          
                DurationDays = p.DurationDays,
                ImageUrl = p.ImageUrl!
            })
            .ToListAsync();
    }

    // GET BY ID
    public async Task<PackageDto?> GetByIdAsync(int id)
    {
        return await _context.Packages
    .Where(p => p.PackageId == id && p.IsActive)
    .Select(p => new PackageDto
    {
        Id = p.PackageId,
        Title = p.Name,
        Price = p.BasePrice,
        DurationDays = p.DurationDays,
        ImageUrl = p.ImageUrl!
    })
    .FirstOrDefaultAsync();
    }

    // CREATE
    public async Task<int> CreateAsync(CreatePackageRequest request)
    {
        var package = new Package
        {
            DestinationId = request.DestinationId,
            Name = request.Title,                      //  mapping
            ShortDescription = request.Description,    
            BasePrice = request.Price,                 
            DurationDays = request.DurationDays,
            ImageUrl = request.ImageUrl,
            Currency = "USD",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            MaxGuests = request.MaxGuests,
        };

        _context.Packages.Add(package);
        await _context.SaveChangesAsync();

        return package.PackageId;
    }

    // UPDATE
    public async Task<bool> UpdateAsync(int id, UpdatePackageRequest request)
    {
        var package = await _context.Packages.FindAsync(id);
        if (package == null) return false;

        package.Name = request.Title;
        package.ShortDescription = request.Description;
        package.BasePrice = request.Price;
        package.DurationDays = request.DurationDays;
        package.ImageUrl = request.ImageUrl;
        package.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    // DELETE (Soft Delete)
    public async Task<bool> DeleteAsync(int id)
    {
        var package = await _context.Packages.FindAsync(id);
        if (package == null) return false;

        package.IsActive = false;

        await _context.SaveChangesAsync();
        return true;
    }
}