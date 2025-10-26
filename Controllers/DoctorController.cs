using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareAppointmentSystem.Data;
using HealthcareAppointmentSystem.Models;

namespace HealthcareAppointmentSystem.Controllers;

[Authorize(Roles = "Doctor")]
public class DoctorController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DoctorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var doctor = await _context.Doctors
            .Include(d => d.Specialty)
            .FirstOrDefaultAsync(d => d.UserId == user!.Id);

        if (doctor == null) return NotFound();

        var upcomingAppointments = await _context.Appointments
            .Include(a => a.Patient)
            .ThenInclude(p => p!.User)
            .Where(a => a.DoctorId == doctor.Id && a.AppointmentDate >= DateTime.Today)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();

        ViewBag.Doctor = doctor;
        return View(upcomingAppointments);
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        var doctor = await _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Specialty)
            .FirstOrDefaultAsync(d => d.UserId == user!.Id);

        if (doctor == null) return NotFound();

        ViewBag.Specialties = await _context.Specialties.ToListAsync();
        return View(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> Profile(int id, string qualifications, string bio, decimal consultationFee, 
        string location, string workingHours, bool isAvailable)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) return NotFound();

        doctor.Qualifications = qualifications;
        doctor.Bio = bio;
        doctor.ConsultationFee = consultationFee;
        doctor.Location = location;
        doctor.WorkingHours = workingHours;
        doctor.IsAvailable = isAvailable;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Appointments()
    {
        var user = await _userManager.GetUserAsync(User);
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == user!.Id);

        if (doctor == null) return NotFound();

        var appointments = await _context.Appointments
            .Include(a => a.Patient)
            .ThenInclude(p => p!.User)
            .Where(a => a.DoctorId == doctor.Id)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        return View(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAppointmentStatus(int id, AppointmentStatus status)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            appointment.Status = status;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Appointments));
    }

    [HttpGet]
    public async Task<IActionResult> UpdateNotes(int id)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Patient)
            .ThenInclude(p => p!.User)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment == null) return NotFound();
        return View(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateNotes(int id, string consultationNotes, string prescription)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            appointment.ConsultationNotes = consultationNotes;
            appointment.Prescription = prescription;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Appointments));
    }
}
