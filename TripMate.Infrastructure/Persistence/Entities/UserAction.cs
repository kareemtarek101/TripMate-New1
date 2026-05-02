using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

public partial class UserAction
{
    [Key]
    [Column("action_id")]
    public int ActionId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("destination_id")]
    public int? DestinationId { get; set; }

    [Column("package_id")]
    public int? PackageId { get; set; }

    [Column("flight_id")]
    public int? FlightId { get; set; }

    [Column("action_type")]
    [StringLength(50)]
    public string ActionType { get; set; } = null!;

    [Column("action_value", TypeName = "decimal(5, 2)")]
    public decimal? ActionValue { get; set; }

    [Column("action_timestamp")]
    public DateTime ActionTimestamp { get; set; }

    [ForeignKey("DestinationId")]
    [InverseProperty("UserActions")]
    public virtual Destination? Destination { get; set; }

    [ForeignKey("FlightId")]
    [InverseProperty("UserActions")]
    public virtual Flight? Flight { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("UserActions")]
    public virtual Package? Package { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserActions")]
    public virtual User User { get; set; } = null!;
}
