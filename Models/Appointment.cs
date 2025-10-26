using System.ComponentModel.DataAnnotations;

namespace HealthcareAppointmentSystem.Models;

public enum AppointmentStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled,
    Rescheduled
}

public class Appointment
{
    public int Id { get; set; }
    
    [Required]
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    
    [Required]
    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    [Required]
    [StringLength(50)]
    public string TimeSlot { get; set; } = string.Empty;
    
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    
    [StringLength(500)]
    public string? Reason { get; set; }
    
    [StringLength(2000)]
    public string? ConsultationNotes { get; set; }
    
    [StringLength(2000)]
    public string? Prescription { get; set; }
    
    public int? PaymentId { get; set; }
    public Payment? Payment { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
