using Microsoft.EntityFrameworkCore;
using TripMate.Infrastructure.Persistence;
using TripMate.Infrastructure.Persistence.Entities;

public class PostService : IPostService
{
    private readonly TripMateDbContext _context;

    public PostService(TripMateDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(int userId, CreatePostRequest request)
    {
        var post = new Post
        {
            UserId = userId,
            Title = request.Title,
            Location = request.Location,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Rating = request.Rating
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return post.PostId;
    }
    public async Task<List<CreatePostRequest>> GetAllAsync()
    {
        return await _context.Posts
            .OrderByDescending(p => p.PostId)
            .Select(p => new CreatePostRequest
            {
                PostId = p.PostId,
                Title = p.Title,
                Location = p.Location,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Rating = p.Rating,
                UserId = p.UserId
            })
            .ToListAsync();
    }
    public async Task<CreatePostRequest?> GetByIdAsync(int id)
    {
        return await _context.Posts
            .Where(p => p.PostId == id)
            .Select(p => new CreatePostRequest
            {
                PostId = p.PostId,
                UserId = p.UserId,
                Title = p.Title,
                Location = p.Location,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Rating = p.Rating
            })
            .FirstOrDefaultAsync();
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
            return false;

        _context.Posts.Remove(post);

        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<bool> UpdateAsync(int id, CreatePostRequest request)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
            return false;

        post.Title = request.Title;
        post.Location = request.Location;
        post.Description = request.Description;
        post.ImageUrl = request.ImageUrl;
        post.Rating = request.Rating;

        await _context.SaveChangesAsync();

        return true;
    }
}