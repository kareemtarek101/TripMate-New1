using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMate.Infrastructure.Persistence.Entities;

public class Favorite
{
    [Key]
    [Column("favorite_id")]
    public int FavoriteId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("item_id")]
    public int ItemId { get; set; }

    [Column("item_type")]
    public string ItemType { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ✅ بس User
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}