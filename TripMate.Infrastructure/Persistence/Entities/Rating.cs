using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMate.Infrastructure.Persistence.Entities;

public class Rating
{
    [Key]
    [Column("rating_id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    // 🔥 Generic Item (Destination / Hotel / Flight / Package)
    [Column("item_id")]
    public int ItemId { get; set; }

    [Column("item_type")]
    [StringLength(50)]
    public string ItemType { get; set; } = null!;

    // ⭐ من 1 لـ 5
    [Column("value")]
    [Range(1, 5)]
    public int Value { get; set; }

    [Column("comment")]
    [StringLength(500)]
    public string? Comment { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 🔗 Navigation
    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(User.Ratings))]
    public virtual User User { get; set; } = null!;
}