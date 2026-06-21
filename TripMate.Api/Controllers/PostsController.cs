using Microsoft.AspNetCore.Mvc;
using TripMate.Application;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        int userId = int.Parse(userIdClaim);
        if (request.Rating < 1 || request.Rating > 5)
            return BadRequest("Rating must be between 1 and 5");

        var id = await _postService.CreateAsync(userId, request);

        return Ok(new
        {
            message = "Post created",
            postId = id
        });
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var posts = await _postService.GetAllAsync();

        return Ok(posts);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var post = await _postService.GetByIdAsync(id);

        if (post == null)
            return NotFound();

        return Ok(post);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _postService.DeleteAsync(id);

        if (!ok)
            return NotFound();

        return Ok(new
        {
            message = "Post deleted"
        });
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
    int id,
    [FromBody] CreatePostRequest request)
    {
        var ok = await _postService.UpdateAsync(id, request);

        if (!ok)
            return NotFound();

        return Ok(new
        {
            message = "Post updated"
        });
    }
}