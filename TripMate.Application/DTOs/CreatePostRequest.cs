public class CreatePostRequest
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int Rating { get; set; }
    public int UserId { get; set; }

}