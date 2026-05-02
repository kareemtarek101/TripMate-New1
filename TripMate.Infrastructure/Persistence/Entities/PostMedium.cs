using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

public partial class PostMedium
{
    [Key]
    [Column("media_id")]
    public int MediaId { get; set; }

    [Column("post_id")]
    public int PostId { get; set; }

    [Column("media_url")]
    [StringLength(255)]
    public string MediaUrl { get; set; } = null!;

    [Column("media_type")]
    [StringLength(50)]
    public string MediaType { get; set; } = null!;

    [Column("sort_order")]
    public int SortOrder { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("PostMedia")]
    public virtual Post Post { get; set; } = null!;
}
