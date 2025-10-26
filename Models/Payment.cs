using System.ComponentModel.DataAnnotations;

namespace HealthcareAppointmentSystem.Models;

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded
}

public class Payment
{
    public int Id { get; set; }
    
    [Required]
    public int AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
    
    [Required]
    [Range(0, 100000)]
    public decimal Amount { get; set; }
    
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    
    [StringLength(100)]
    public string? StripePaymentIntentId { get; set; }
    
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    
    [StringLength(500)]
    public string? TransactionDetails { get; set; }
}
