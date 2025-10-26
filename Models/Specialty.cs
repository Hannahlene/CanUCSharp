using System.ComponentModel.DataAnnotations;

namespace HealthcareAppointmentSystem.Models;

public class Specialty
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
