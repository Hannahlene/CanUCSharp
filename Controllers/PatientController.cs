using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareAppointmentSystem.Data;
using HealthcareAppointmentSystem.Models;

namespace HealthcareAppointmentSystem.Controllers;

[Authorize(Roles = "Patient")]
public class PatientController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PatientController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user!.Id);

        if (patient == null)
        {
            patient = new Patient
            {
                UserId = user!.Id,
                DateOfBirth = DateTime.UtcNow.AddYears(-30)
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        var upcomingAppointments = await _context.Appointments
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.User)
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.Specialty)
            .Where(a => a.PatientId == patient.Id && a.AppointmentDate >= DateTime.Today)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();

        return View(upcomingAppointments);
    }

    public async Task<IActionResult> SearchDoctors(string? specialty, string? location)
    {
        var query = _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Specialty)
            .Where(d => d.IsAvailable);

        if (!string.IsNullOrEmpty(specialty))
        {
            query = query.Where(d => d.Specialty != null && d.Specialty.Name.Contains(specialty));
        }

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(d => d.Location != null && d.Location.Contains(location));
        }

        var doctors = await query.ToListAsync();
        ViewBag.Specialties = await _context.Specialties.ToListAsync();
        return View(doctors);
    }

    [HttpGet]
    public async Task<IActionResult> BookAppointment(int doctorId)
    {
        var doctor = await _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Specialty)
            .FirstOrDefaultAsync(d => d.Id == doctorId);

        if (doctor == null) return NotFound();

        ViewBag.Doctor = doctor;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> BookAppointment(int doctorId, DateTime appointmentDate, string timeSlot, string reason)
    {
        var user = await _userManager.GetUserAsync(User);
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user!.Id);

        if (patient == null) return NotFound();

        var appointment = new Appointment
        {
            PatientId = patient.Id,
            DoctorId = doctorId,
            AppointmentDate = appointmentDate,
            TimeSlot = timeSlot,
            Reason = reason,
            Status = AppointmentStatus.Pending
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(BookingSuccess));
    }

    public IActionResult BookingSuccess()
    {
        return View();
    }

    public async Task<IActionResult> Appointments()
    {
        var user = await _userManager.GetUserAsync(User);
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user!.Id);

        if (patient == null) return NotFound();

        var appointments = await _context.Appointments
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.User)
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.Specialty)
            .Where(a => a.PatientId == patient.Id)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        return View(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> CancelAppointment(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user!.Id);

        if (patient == null) return NotFound();

        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == id && a.PatientId == patient.Id);

        if (appointment == null) return NotFound();

        appointment.Status = AppointmentStatus.Cancelled;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Appointments));
    }

    [HttpGet]
    public async Task<IActionResult> LeaveFeedback(int appointmentId)
    {
        var user = await _userManager.GetUserAsync(User);
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user!.Id);

        if (patient == null) return NotFound();

        var appointment = await _context.Appointments
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.User)
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.Specialty)
            .FirstOrDefaultAsync(a => a.Id == appointmentId && a.PatientId == patient.Id && a.Status == AppointmentStatus.Completed);

        if (appointment == null) return NotFound();

        var existingFeedback = await _context.Feedbacks
            .FirstOrDefaultAsync(f => f.AppointmentId == appointmentId);

        if (existingFeedback != null)
        {
            TempData["ErrorMessage"] = "You have already left feedback for this appointment.";
            return RedirectToAction(nameof(Appointments));
        }

        ViewBag.Appointment = appointment;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LeaveFeedback(int appointmentId, int rating, string? comments)
    {
        var user = await _userManager.GetUserAsync(User);
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user!.Id);

        if (patient == null) return NotFound();

        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == appointmentId && a.PatientId == patient.Id && a.Status == AppointmentStatus.Completed);

        if (appointment == null) return NotFound();

        var existingFeedback = await _context.Feedbacks
            .FirstOrDefaultAsync(f => f.AppointmentId == appointmentId);

        if (existingFeedback != null)
        {
            TempData["ErrorMessage"] = "You have already left feedback for this appointment.";
            return RedirectToAction(nameof(Appointments));
        }

        var feedback = new Feedback
        {
            PatientId = patient.Id,
            DoctorId = appointment.DoctorId,
            AppointmentId = appointmentId,
            Rating = rating,
            Comments = comments
        };

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thank you for your feedback!";
        return RedirectToAction(nameof(Appointments));
    }

    public async Task<IActionResult> MyFeedbacks()
    {
        var user = await _userManager.GetUserAsync(User);
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user!.Id);

        if (patient == null) return NotFound();

        var feedbacks = await _context.Feedbacks
            .Include(f => f.Doctor)
            .ThenInclude(d => d!.User)
            .Include(f => f.Doctor)
            .ThenInclude(d => d!.Specialty)
            .Include(f => f.Appointment)
            .Where(f => f.PatientId == patient.Id)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();

        return View(feedbacks);
    }

    public async Task<IActionResult> PaymentHistory()
    {
        var user = await _userManager.GetUserAsync(User);
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == user!.Id);

        if (patient == null) return NotFound();

        var payments = await _context.Payments
            .Include(p => p.Appointment)
            .ThenInclude(a => a!.Doctor)
            .ThenInclude(d => d!.User)
            .Include(p => p.Appointment)
            .ThenInclude(a => a!.Doctor)
            .ThenInclude(d => d!.Specialty)
            .Where(p => p.Appointment!.PatientId == patient.Id)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();

        var totalPaid = payments
            .Where(p => p.Status == PaymentStatus.Completed)
            .Sum(p => p.Amount);

        ViewBag.TotalPaid = totalPaid;

        return View(payments);
    }
}
