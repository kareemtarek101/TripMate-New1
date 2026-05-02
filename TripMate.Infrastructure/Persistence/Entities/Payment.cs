using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripMate.Infrastructure.Persistence.Entities;

public partial class Payment
{
    [Key]
    [Column("payment_id")]
    public int PaymentId { get; set; }

    [Column("booking_id")]
    public int BookingId { get; set; }

    [Column("amount", TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    [Column("currency")]
    [StringLength(10)]
    public string Currency { get; set; } = null!;

    [Column("gateway_type")]
    [StringLength(20)]
    public string GatewayType { get; set; } = null!;

    [Column("transaction_status")]
    [StringLength(20)]
    public string TransactionStatus { get; set; } = null!;

    [Column("transaction_reference")]
    [StringLength(100)]
    public string? TransactionReference { get; set; }

    [Column("paid_at")]
    public DateTime PaidAt { get; set; }

    [ForeignKey("BookingId")]
    [InverseProperty("Payments")]
    public virtual Booking Booking { get; set; } = null!;
}
