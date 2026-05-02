using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

public partial class Post
{
    [Key]
    [Column("post_id")]
    public int PostId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("destination_id")]
    public int? DestinationId { get; set; }

    [Column("package_id")]
    public int? PackageId { get; set; }

    [Column("title")]
    [StringLength(150)]
    public string Title { get; set; } = null!;

    [Column("content")]
    public string? Content { get; set; }

    [Column("trip_type")]
    [StringLength(50)]
    public string? TripType { get; set; }

    [Column("rating")]
    public int? Rating { get; set; }

    [Column("is_public")]
    public bool IsPublic { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("DestinationId")]
    [InverseProperty("Posts")]
    public virtual Destination? Destination { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("Posts")]
    public virtual Package? Package { get; set; }

    [InverseProperty("Post")]
    public virtual ICollection<PostMedium> PostMedia { get; set; } = new List<PostMedium>();

    [ForeignKey("UserId")]
    [InverseProperty("Posts")]
    public virtual User User { get; set; } = null!;
}
