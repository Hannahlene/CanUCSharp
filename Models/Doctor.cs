using System.ComponentModel.DataAnnotations;

namespace HealthcareAppointmentSystem.Models;

public class Doctor
{
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
    
    [Required]
    public int SpecialtyId { get; set; }
    public Specialty? Specialty { get; set; }
    
    [StringLength(200)]
    public string? Qualifications { get; set; }
    
    [StringLength(500)]
    public string? Bio { get; set; }
    
    [Required]
    [Range(0, 100000)]
    public decimal ConsultationFee { get; set; }
    
    [StringLength(200)]
    public string? Location { get; set; }
    
    public bool IsAvailable { get; set; } = true;
    
    public string? WorkingHours { get; set; }
    
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
