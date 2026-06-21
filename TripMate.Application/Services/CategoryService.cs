using TripMate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
public class CategoryService : ICategoryService
{
    private readonly TripMateDbContext _context;

    public CategoryService(TripMateDbContext context)
    {
        _context = context;
    }
    public async Task<int> CreateAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category.Id; 
    }
    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }
}