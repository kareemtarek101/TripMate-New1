using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMate.Infrastructure.Persistence.Entities;

public class Post
{
    [Key]
    [Column("post_id")]
    public int PostId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    // 🔥 Title
    [Column("title")]
    [StringLength(150)]
    public string Title { get; set; } = null!;

    // 🔥 Location (بدل Destination دلوقتي)
    [Column("location")]
    [StringLength(150)]
    public string Location { get; set; } = null!;

    // 🔥 Description (بدل Content)
    [Column("description")]
    public string? Description { get; set; }

    // 🔥 صورة واحدة بسيطة
    [Column("image_url")]
    public string ImageUrl { get; set; } = null!;

    // ⭐ Rating
    [Column("rating")]
    [Range(1, 5)]
    public int Rating { get; set; }

    // 🌍 Public / Private
    [Column("is_public")]
    public bool IsPublic { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 🔗 User
    [ForeignKey("UserId")]
    [InverseProperty("Posts")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("DestinationId")]
    [InverseProperty("Posts")]
    public virtual Destination? Destination { get; set; }

    [InverseProperty("Post")]
    public virtual ICollection<PostMedium> PostMedia { get; set; } = new List<PostMedium>();
}