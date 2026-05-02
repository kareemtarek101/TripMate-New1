using TripMate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
public class CategoryService : ICategoryService
{
    private readonly TripMateDbContext _context;

    public CategoryService(TripMateDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }
}