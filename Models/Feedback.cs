using System.ComponentModel.DataAnnotations;

namespace HealthcareAppointmentSystem.Models;

public class Feedback
{
    public int Id { get; set; }
    
    [Required]
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    
    [Required]
    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    
    public int? AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
    
    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }
    
    [StringLength(1000)]
    public string? Comments { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
