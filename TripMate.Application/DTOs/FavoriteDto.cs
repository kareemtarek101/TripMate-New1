public class FavoriteDto
{

    public int FavoriteId { get; set; }
    public int UserId { get; set; }

    public int ItemId { get; set; }
    public string ItemType { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}