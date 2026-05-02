using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

[Index("UserId", Name = "UQ_UserPreferences_User", IsUnique = true)]
public partial class UserPreference
{
    [Key]
    [Column("user_pref_id")]
    public int UserPrefId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("preferred_trip_type_id")]
    public int? PreferredTripTypeId { get; set; }

    [Column("min_budget", TypeName = "decimal(10, 2)")]
    public decimal? MinBudget { get; set; }

    [Column("max_budget", TypeName = "decimal(10, 2)")]
    public decimal? MaxBudget { get; set; }

    [Column("preferred_season")]
    [StringLength(50)]
    public string? PreferredSeason { get; set; }

    [Column("preferred_airlines")]
    [StringLength(255)]
    public string? PreferredAirlines { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("PreferredTripTypeId")]
    [InverseProperty("UserPreferences")]
    public virtual TripType? PreferredTripType { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserPreference")]
    public virtual User User { get; set; } = null!;
}
