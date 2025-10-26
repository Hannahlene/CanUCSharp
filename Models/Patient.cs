using System.ComponentModel.DataAnnotations;

namespace HealthcareAppointmentSystem.Models;

public class Patient
{
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    [StringLength(10)]
    public string? Gender { get; set; }
    
    [StringLength(200)]
    public string? EmergencyContact { get; set; }
    
    [StringLength(1000)]
    public string? MedicalHistory { get; set; }
    
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
