using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

public partial class TripType
{
    [Key]
    [Column("trip_type_id")]
    public int TripTypeId { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(255)]
    public string? Description { get; set; }

    [InverseProperty("TripType")]
    public virtual ICollection<Destination> Destinations { get; set; } = new List<Destination>();

    [InverseProperty("PreferredTripType")]
    public virtual ICollection<UserPreference> UserPreferences { get; set; } = new List<UserPreference>();
}
