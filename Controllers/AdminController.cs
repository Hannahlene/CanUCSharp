using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareAppointmentSystem.Data;
using HealthcareAppointmentSystem.Models;

namespace HealthcareAppointmentSystem.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.DoctorCount = await _context.Doctors.CountAsync();
        ViewBag.PatientCount = await _context.Patients.CountAsync();
        ViewBag.AppointmentCount = await _context.Appointments.CountAsync();
        return View();
    }

    public async Task<IActionResult> Doctors()
    {
        var doctors = await _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Specialty)
            .ToListAsync();
        return View(doctors);
    }

    [HttpGet]
    public async Task<IActionResult> AddDoctor()
    {
        ViewBag.Specialties = await _context.Specialties.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddDoctor(string email, string password, string firstName, string lastName, 
        int specialtyId, string qualifications, string bio, decimal consultationFee, string location, string workingHours)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Doctor");

            var doctor = new Doctor
            {
                UserId = user.Id,
                SpecialtyId = specialtyId,
                Qualifications = qualifications,
                Bio = bio,
                ConsultationFee = consultationFee,
                Location = location,
                WorkingHours = workingHours,
                IsAvailable = true
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Doctors));
        }

        ViewBag.Specialties = await _context.Specialties.ToListAsync();
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> EditDoctor(int id)
    {
        var doctor = await _context.Doctors
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (doctor == null) return NotFound();

        ViewBag.Specialties = await _context.Specialties.ToListAsync();
        return View(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> EditDoctor(int id, int specialtyId, string qualifications, string bio, 
        decimal consultationFee, string location, string workingHours, bool isAvailable)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) return NotFound();

        doctor.SpecialtyId = specialtyId;
        doctor.Qualifications = qualifications;
        doctor.Bio = bio;
        doctor.ConsultationFee = consultationFee;
        doctor.Location = location;
        doctor.WorkingHours = workingHours;
        doctor.IsAvailable = isAvailable;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Doctors));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor != null)
        {
            var user = await _userManager.FindByIdAsync(doctor.UserId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Doctors));
    }

    public async Task<IActionResult> Patients()
    {
        var patients = await _context.Patients
            .Include(p => p.User)
            .ToListAsync();
        return View(patients);
    }

    public async Task<IActionResult> Specialties()
    {
        var specialties = await _context.Specialties.ToListAsync();
        return View(specialties);
    }

    [HttpPost]
    public async Task<IActionResult> AddSpecialty(string name, string description)
    {
        var specialty = new Specialty
        {
            Name = name,
            Description = description
        };

        _context.Specialties.Add(specialty);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Specialties));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSpecialty(int id)
    {
        var specialty = await _context.Specialties.FindAsync(id);
        if (specialty != null)
        {
            _context.Specialties.Remove(specialty);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Specialties));
    }

    public async Task<IActionResult> Reports()
    {
        var totalDoctors = await _context.Doctors.CountAsync();
        var totalPatients = await _context.Patients.CountAsync();
        var totalAppointments = await _context.Appointments.CountAsync();
        var pendingAppointments = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Pending);
        var confirmedAppointments = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Confirmed);
        var completedAppointments = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Completed);
        var cancelledAppointments = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Cancelled);
        
        var totalRevenue = await _context.Payments
            .Where(p => p.Status == PaymentStatus.Completed)
            .SumAsync(p => (decimal?)p.Amount) ?? 0;

        var appointmentsBySpecialty = await _context.Appointments
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Specialty)
            .GroupBy(a => a.Doctor!.Specialty!.Name)
            .Select(g => new { Specialty = g.Key, Count = g.Count() })
            .ToListAsync();

        var revenueBySpecialty = await _context.Payments
            .Where(p => p.Status == PaymentStatus.Completed)
            .Include(p => p.Appointment)
            .ThenInclude(a => a!.Doctor)
            .ThenInclude(d => d!.Specialty)
            .GroupBy(p => p.Appointment!.Doctor!.Specialty!.Name)
            .Select(g => new { Specialty = g.Key, Revenue = g.Sum(p => p.Amount) })
            .ToListAsync();

        ViewBag.TotalDoctors = totalDoctors;
        ViewBag.TotalPatients = totalPatients;
        ViewBag.TotalAppointments = totalAppointments;
        ViewBag.PendingAppointments = pendingAppointments;
        ViewBag.ConfirmedAppointments = confirmedAppointments;
        ViewBag.CompletedAppointments = completedAppointments;
        ViewBag.CancelledAppointments = cancelledAppointments;
        ViewBag.TotalRevenue = totalRevenue;
        ViewBag.AppointmentsBySpecialty = appointmentsBySpecialty;
        ViewBag.RevenueBySpecialty = revenueBySpecialty;

        return View();
    }

    public async Task<IActionResult> AppointmentReport()
    {
        var appointments = await _context.Appointments
            .Include(a => a.Patient)
            .ThenInclude(p => p!.User)
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.User)
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.Specialty)
            .Include(a => a.Payment)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        return View(appointments);
    }

    public async Task<IActionResult> PaymentReport()
    {
        var payments = await _context.Payments
            .Include(p => p.Appointment)
            .ThenInclude(a => a!.Patient)
            .ThenInclude(p => p!.User)
            .Include(p => p.Appointment)
            .ThenInclude(a => a!.Doctor)
            .ThenInclude(d => d!.User)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();

        return View(payments);
    }

    public async Task<IActionResult> PatientStatistics()
    {
        var patientStats = await _context.Patients
            .Include(p => p.User)
            .Select(p => new
            {
                Patient = p,
                TotalAppointments = p.Appointments.Count,
                CompletedAppointments = p.Appointments.Count(a => a.Status == AppointmentStatus.Completed),
                CancelledAppointments = p.Appointments.Count(a => a.Status == AppointmentStatus.Cancelled),
                TotalSpent = p.Appointments
                    .Where(a => a.Payment != null && a.Payment.Status == PaymentStatus.Completed)
                    .Sum(a => (decimal?)a.Payment!.Amount) ?? 0
            })
            .OrderByDescending(ps => ps.TotalAppointments)
            .ToListAsync();

        return View(patientStats);
    }
}
