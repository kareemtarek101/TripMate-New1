public class CreateRatingRequest
{
    public int ItemId { get; set; }
    public string ItemType { get; set; }
    public int Value { get; set; }
    public string? Comment { get; set; }
}