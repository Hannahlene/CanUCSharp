using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareAppointmentSystem.Data;
using HealthcareAppointmentSystem.Models;
using Stripe;
using Stripe.Checkout;

namespace HealthcareAppointmentSystem.Controllers;

[Authorize(Roles = "Patient")]
public class PatientController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public PatientController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
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

        return RedirectToAction(nameof(Payment), new { appointmentId = appointment.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Payment(int appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.User)
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.Specialty)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        if (appointment == null) return NotFound();

        return View(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCheckoutSession(int appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        if (appointment == null || appointment.Doctor == null) return NotFound();

        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"] ?? "sk_test_placeholder";

        var domain = $"{Request.Scheme}://{Request.Host}";

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(appointment.Doctor.ConsultationFee * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Consultation with Dr. {appointment.Doctor.User?.FirstName} {appointment.Doctor.User?.LastName}",
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = domain + $"/Patient/PaymentSuccess?appointmentId={appointmentId}",
            CancelUrl = domain + $"/Patient/Payment?appointmentId={appointmentId}",
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        return Redirect(session.Url);
    }

    public async Task<IActionResult> PaymentSuccess(int appointmentId)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment != null)
        {
            var payment = new Payment
            {
                AppointmentId = appointmentId,
                Amount = (await _context.Doctors.FindAsync(appointment.DoctorId))!.ConsultationFee,
                Status = PaymentStatus.Completed,
                TransactionDetails = "Paid via Stripe"
            };

            _context.Payments.Add(payment);
            appointment.PaymentId = payment.Id;
            appointment.Status = AppointmentStatus.Confirmed;
            await _context.SaveChangesAsync();
        }

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
            .Include(a => a.Payment)
            .Where(a => a.PatientId == patient.Id)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        return View(appointments);
    }
}
