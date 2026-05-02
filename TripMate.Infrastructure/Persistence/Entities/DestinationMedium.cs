using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

public partial class DestinationMedium
{
    [Key]
    [Column("media_id")]
    public int MediaId { get; set; }

    [Column("destination_id")]
    public int DestinationId { get; set; }

    [Column("media_url")]
    [StringLength(255)]
    public string MediaUrl { get; set; } = null!;

    [Column("media_type")]
    [StringLength(50)]
    public string MediaType { get; set; } = null!;

    [Column("sort_order")]
    public int SortOrder { get; set; }

    [ForeignKey("DestinationId")]
    [InverseProperty("DestinationMedia")]
    public virtual Destination Destination { get; set; } = null!;
}
