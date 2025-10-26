using Microsoft.AspNetCore.Identity;

namespace HealthcareAppointmentSystem.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
}
