using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

public partial class RecommendationLog
{
    [Key]
    [Column("rec_log_id")]
    public int RecLogId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("destination_id")]
    public int? DestinationId { get; set; }

    [Column("package_id")]
    public int? PackageId { get; set; }

    [Column("flight_id")]
    public int? FlightId { get; set; }

    [Column("algorithm_version")]
    [StringLength(50)]
    public string AlgorithmVersion { get; set; } = null!;

    [Column("score", TypeName = "decimal(5, 3)")]
    public decimal? Score { get; set; }

    [Column("shown_at")]
    public DateTime ShownAt { get; set; }

    [Column("clicked")]
    public bool Clicked { get; set; }

    [ForeignKey("DestinationId")]
    [InverseProperty("RecommendationLogs")]
    public virtual Destination? Destination { get; set; }

    [ForeignKey("FlightId")]
    [InverseProperty("RecommendationLogs")]
    public virtual Flight? Flight { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("RecommendationLogs")]
    public virtual Package? Package { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("RecommendationLogs")]
    public virtual User User { get; set; } = null!;
}
