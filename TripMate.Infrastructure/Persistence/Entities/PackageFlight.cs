using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

public partial class PackageFlight
{
    [Key]
    [Column("package_flight_id")]
    public int PackageFlightId { get; set; }

    [Column("package_id")]
    public int PackageId { get; set; }

    [Column("flight_id")]
    public int FlightId { get; set; }

    [Column("flight_role")]
    [StringLength(20)]
    public string FlightRole { get; set; } = null!;

    [Column("price_difference", TypeName = "decimal(10, 2)")]
    public decimal? PriceDifference { get; set; }

    [ForeignKey("FlightId")]
    [InverseProperty("PackageFlights")]
    public virtual Flight Flight { get; set; } = null!;

    [ForeignKey("PackageId")]
    [InverseProperty("PackageFlights")]
    public virtual Package Package { get; set; } = null!;
}
