# Healthcare Appointment System - Implementation Documentation

## Table of Contents
1. System Overview
2. Technology Stack
3. Implemented Features
4. Screen Captures and Functionality
5. Database Implementation
6. Security Features
7. Code Structure

---

## 1. System Overview

The Healthcare Appointment System is a complete web-based solution developed for MediCare Pvt Ltd to manage healthcare appointments across multiple clinics and medical centers in Sri Lanka. The system implements role-based access control with three distinct user types: Administrators, Doctors, and Patients.

**Development Framework:** ASP.NET Core 8.0 MVC  
**Database:** SQLite with Entity Framework Core  
**Authentication:** ASP.NET Core Identity  
**UI Framework:** Bootstrap 5  
**Development Environment:** Replit Cloud IDE

---

## 2. Technology Stack

### Backend Technologies
- **ASP.NET Core 8.0:** Web application framework
- **Entity Framework Core:** Object-Relational Mapping (ORM)
- **ASP.NET Core Identity:** User authentication and authorization
- **C# 12:** Programming language
- **SQLite:** Embedded database system

### Frontend Technologies
- **Razor View Engine:** Server-side rendering
- **Bootstrap 5:** Responsive CSS framework
- **jQuery:** JavaScript library for DOM manipulation
- **jQuery Validation:** Client-side form validation

### Development Tools
- **Replit:** Cloud-based development environment
- **Git:** Version control system
- **.NET CLI:** Command-line interface for .NET development

---

## 3. Implemented Features

### 3.1 Authentication & Authorization
✓ User registration for patients  
✓ Login/logout functionality  
✓ Role-based access control (Admin, Doctor, Patient)  
✓ Password hashing and security  
✓ Session management  

### 3.2 Admin Features
✓ Dashboard with system statistics  
✓ Add/Edit/Delete doctor profiles  
✓ Manage medical specialties  
✓ View all registered patients  
✓ Generate comprehensive reports:
  - Overall system statistics
  - Appointment reports
  - Payment reports
  - Patient statistics

### 3.3 Doctor Features
✓ Personal dashboard with upcoming appointments  
✓ Update profile and availability  
✓ View all appointments  
✓ Confirm/cancel appointments  
✓ Add consultation notes  
✓ Add prescriptions for patients  

### 3.4 Patient Features
✓ Search doctors by specialty  
✓ Search doctors by location  
✓ Book appointments  
✓ View appointment history  
✓ Cancel appointments  
✓ View payment history and transactions  
✓ Track total amount spent  
✓ Leave feedback and ratings for doctors  
✓ View submitted feedbacks  

### 3.5 Feedback System
✓ 5-star rating system  
✓ Written comments/reviews  
✓ One feedback per appointment  
✓ Visible to patients who left them  

### 3.6 Reporting System
✓ System-wide statistics dashboard  
✓ Appointment analytics by specialty  
✓ Revenue tracking and reports  
✓ Patient engagement metrics  

---

## 4. Screen Captures and Functionality

### 4.1 Home Page
**File:** `Views/Home/Index.cshtml`  
**Route:** `/`

**Screenshot Description:**
The home page displays a clean, professional interface with:
- MediCare Healthcare branding in the header
- Welcome message and tagline
- Three distinct sections:
  - **For Patients:** Register or sign in to book appointments
  - **For Doctors:** Sign in to manage appointments
  - **For Administrators:** Sign in to manage the system
- List of available medical specialties
- Navigation links for Register and Login

**Key Features:**
- Responsive design that works on all devices
- Clear call-to-action buttons
- Informative specialty cards
- Easy navigation

---

### 4.2 Patient Registration
**File:** `Views/Account/Register.cshtml`  
**Route:** `/Account/Register`

**Screenshot Description:**
Registration form includes:
- Email address (will become username)
- Password field with requirements
- Confirm password field
- First name and last name
- Phone number (optional)
- Address (optional)
- Register button
- Link to sign in if already registered

**Implementation Details:**
```csharp
// Controller: AccountController.cs
[HttpPost]
public async Task<IActionResult> Register(RegisterViewModel model)
{
    var user = new ApplicationUser
    {
        UserName = model.Email,
        Email = model.Email,
        FirstName = model.FirstName,
        LastName = model.LastName,
        PhoneNumber = model.PhoneNumber,
        Address = model.Address
    };
    
    var result = await _userManager.CreateAsync(user, model.Password);
    if (result.Succeeded)
    {
        await _userManager.AddToRoleAsync(user, "Patient");
        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToAction("Index", "Patient");
    }
}
```

---

### 4.3 Login Page
**File:** `Views/Account/Login.cshtml`  
**Route:** `/Account/Login`

**Screenshot Description:**
Login form with:
- Email address field
- Password field
- "Remember me" checkbox
- Sign in button
- Link to registration page

**Security Features:**
- Password is hashed using ASP.NET Core Identity
- Failed login attempts are tracked
- Account lockout after multiple failed attempts

---

### 4.4 Admin Dashboard
**File:** `Views/Admin/Index.cshtml`  
**Route:** `/Admin/Index`  
**Authorization:** Admin role required

**Screenshot Description:**
Dashboard displays:
- Welcome message with admin name
- Three statistics cards:
  - Total Doctors (with green icon)
  - Total Patients (with blue icon)
  - Total Appointments (with orange icon)
- Navigation menu with links to:
  - Manage Doctors
  - Manage Specialties
  - View Patients
  - Generate Reports

**Code Implementation:**
```csharp
public async Task<IActionResult> Index()
{
    ViewBag.DoctorCount = await _context.Doctors.CountAsync();
    ViewBag.PatientCount = await _context.Patients.CountAsync();
    ViewBag.AppointmentCount = await _context.Appointments.CountAsync();
    return View();
}
```

---

### 4.5 Manage Doctors (Admin)
**File:** `Views/Admin/Doctors.cshtml`  
**Route:** `/Admin/Doctors`

**Screenshot Description:**
Doctor management page shows:
- "Add Doctor" button at the top
- Table of doctors with columns:
  - Name
  - Email
  - Specialty
  - Consultation Fee
  - Location
  - Availability Status
  - Actions (Edit, Delete buttons)

**Functionality:**
- Clicking "Add Doctor" opens the add doctor form
- Edit button allows updating doctor information
- Delete button removes doctor (with confirmation)
- Doctors are displayed with all relevant information

---

### 4.6 Add Doctor Form (Admin)
**File:** `Views/Admin/AddDoctor.cshtml`  
**Route:** `/Admin/AddDoctor`

**Screenshot Description:**
Comprehensive form with fields:
- Email (required)
- Password (required)
- First Name (required)
- Last Name (required)
- Specialty dropdown (required)
- Qualifications (e.g., "MBBS, MD")
- Bio/Professional description
- Consultation Fee (numeric)
- Location
- Working Hours
- Submit button

**Validation:**
- All required fields must be filled
- Email must be unique and valid format
- Password must meet complexity requirements
- Consultation fee must be a positive number

---

### 4.7 Manage Specialties (Admin)
**File:** `Views/Admin/Specialties.cshtml`  
**Route:** `/Admin/Specialties`

**Screenshot Description:**
Specialty management interface:
- List of existing specialties in cards/table:
  - Specialty name
  - Description
  - Delete button
- "Add New Specialty" form at bottom:
  - Specialty name field
  - Description field
  - Add button

**Implementation:**
```csharp
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
```

---

### 4.8 View All Patients (Admin)
**File:** `Views/Admin/Patients.cshtml`  
**Route:** `/Admin/Patients`

**Screenshot Description:**
Patient list displaying:
- Patient name
- Email address
- Phone number
- Date of birth
- Gender
- Emergency contact
- Medical history preview

**Purpose:**
Allows administrators to view all registered patients and their basic information for system management purposes.

---

### 4.9 Admin Reports Dashboard
**File:** `Views/Admin/Reports.cshtml`  
**Route:** `/Admin/Reports`

**Screenshot Description:**
Comprehensive reports dashboard with:
- **Summary Statistics Cards:**
  - Total Doctors
  - Total Patients
  - Total Appointments
  - Total Revenue
- **Appointment Status Breakdown:**
  - Pending appointments count
  - Confirmed appointments count
  - Completed appointments count
  - Cancelled appointments count
- **Charts/Tables:**
  - Appointments by specialty (table or chart)
  - Revenue by specialty
- **Report Links:**
  - View detailed appointment report
  - View payment report
  - View patient statistics

---

### 4.10 Appointment Report (Admin)
**File:** `Views/Admin/AppointmentReport.cshtml`  
**Route:** `/Admin/AppointmentReport`

**Screenshot Description:**
Detailed appointment report table with:
- Appointment ID
- Patient name
- Doctor name
- Specialty
- Appointment date
- Time slot
- Status (color-coded badges)
- Payment status
- Created date

**Features:**
- Sortable columns
- Color-coded status badges:
  - Pending: Yellow
  - Confirmed: Blue
  - Completed: Green
  - Cancelled: Red

---

### 4.11 Payment Report (Admin)
**File:** `Views/Admin/PaymentReport.cshtml`  
**Route:** `/Admin/PaymentReport`

**Screenshot Description:**
Payment transactions table showing:
- Payment ID
- Patient name
- Doctor name
- Amount (LKR)
- Payment status
- Payment date
- Transaction details

**Total Revenue:** Displayed at top or bottom of report

---

### 4.12 Patient Statistics (Admin)
**File:** `Views/Admin/PatientStatistics.cshtml`  
**Route:** `/Admin/PatientStatistics`

**Screenshot Description:**
Patient engagement analytics:
- Patient name
- Total appointments
- Completed appointments
- Cancelled appointments
- Total amount spent
- Sorted by total appointments (most active patients first)

---

### 4.13 Doctor Dashboard
**File:** `Views/Doctor/Index.cshtml`  
**Route:** `/Doctor/Index`  
**Authorization:** Doctor role required

**Screenshot Description:**
Doctor's personal dashboard shows:
- Welcome message with doctor's name
- Profile summary:
  - Specialty
  - Consultation fee
  - Location
  - Working hours
  - Availability status
- **Upcoming Appointments Section:**
  - List of upcoming appointments with:
    - Patient name
    - Date and time
    - Reason for visit
    - Status
    - Action buttons (Confirm, Cancel)

---

### 4.14 Doctor Appointments List
**File:** `Views/Doctor/Appointments.cshtml`  
**Route:** `/Doctor/Appointments`

**Screenshot Description:**
Complete appointments list:
- Filter options (All, Upcoming, Past, Pending, Confirmed, Completed)
- Appointment cards/table showing:
  - Patient information
  - Contact phone
  - Appointment date and time
  - Reason for visit
  - Current status
  - Action buttons based on status:
    - Pending: Confirm, Cancel
    - Confirmed: Cancel, Add Notes
    - Completed: View Notes, Edit Notes

---

### 4.15 Add Consultation Notes (Doctor)
**File:** `Views/Doctor/AddNotes.cshtml` or inline modal  
**Route:** `/Doctor/AddNotes/{appointmentId}`

**Screenshot Description:**
Form to add medical information:
- Patient details (read-only)
- Appointment details (read-only)
- **Consultation Notes** textarea:
  - Symptoms observed
  - Diagnosis
  - Treatment recommendations
- **Prescription** textarea:
  - Medications prescribed
  - Dosage instructions
  - Duration
- Save button

**Code Implementation:**
```csharp
[HttpPost]
public async Task<IActionResult> AddNotes(int id, string consultationNotes, string prescription)
{
    var appointment = await _context.Appointments
        .FirstOrDefaultAsync(a => a.Id == id);
    
    if (appointment == null) return NotFound();
    
    appointment.ConsultationNotes = consultationNotes;
    appointment.Prescription = prescription;
    appointment.Status = AppointmentStatus.Completed;
    
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Appointments));
}
```

---

### 4.16 Update Doctor Profile
**File:** `Views/Doctor/UpdateProfile.cshtml`  
**Route:** `/Doctor/UpdateProfile`

**Screenshot Description:**
Profile update form with:
- Current information pre-filled
- Editable fields:
  - Bio/Professional description
  - Working hours
  - Availability toggle (Available/Not Available)
- Update button

**Note:** Email, name, specialty, and consultation fee can only be changed by admin.

---

### 4.17 Patient Dashboard
**File:** `Views/Patient/Index.cshtml`  
**Route:** `/Patient/Index`  
**Authorization:** Patient role required

**Screenshot Description:**
Patient's personal dashboard displays:
- Welcome message with patient name
- **Upcoming Appointments Section:**
  - Cards showing upcoming appointments:
    - Doctor name and photo (if available)
    - Specialty
    - Date and time
    - Location
    - Status
    - Cancel button
- Quick action buttons:
  - Search for Doctors
  - View All Appointments
  - My Feedbacks

---

### 4.18 Search Doctors (Patient)
**File:** `Views/Patient/SearchDoctors.cshtml`  
**Route:** `/Patient/SearchDoctors`

**Screenshot Description:**
Doctor search interface:
- **Search Filters:**
  - Specialty dropdown (select from list)
  - Location text field
  - Search button
- **Search Results:**
  - Doctor cards showing:
    - Doctor name
    - Qualifications
    - Specialty
    - Consultation fee
    - Location
    - Working hours
    - Availability status
    - "Book Appointment" button

**Implementation:**
```csharp
public async Task<IActionResult> SearchDoctors(string? specialty, string? location)
{
    var query = _context.Doctors
        .Include(d => d.User)
        .Include(d => d.Specialty)
        .Where(d => d.IsAvailable);
    
    if (!string.IsNullOrEmpty(specialty))
        query = query.Where(d => d.Specialty!.Name.Contains(specialty));
    
    if (!string.IsNullOrEmpty(location))
        query = query.Where(d => d.Location!.Contains(location));
    
    var doctors = await query.ToListAsync();
    return View(doctors);
}
```

---

### 4.19 Book Appointment (Patient)
**File:** `Views/Patient/BookAppointment.cshtml`  
**Route:** `/Patient/BookAppointment/{doctorId}`

**Screenshot Description:**
Appointment booking form:
- **Doctor Information (Read-only):**
  - Doctor name
  - Specialty
  - Consultation fee
  - Location
- **Booking Form:**
  - Appointment date picker (future dates only)
  - Time slot field (e.g., "10:00 AM - 11:00 AM")
  - Reason for visit textarea
  - Book Appointment button

**Validation:**
- Date must be in the future
- Time slot is required
- Reason field has character limit

---

### 4.20 Booking Success
**File:** `Views/Patient/BookingSuccess.cshtml`  
**Route:** `/Patient/BookingSuccess`

**Screenshot Description:**
Success confirmation page:
- Success message with checkmark icon
- Appointment details summary
- Message: "Your appointment has been booked successfully. The doctor will confirm it soon."
- Buttons:
  - View My Appointments
  - Search More Doctors
  - Back to Dashboard

---

### 4.21 My Appointments (Patient)
**File:** `Views/Patient/Appointments.cshtml`  
**Route:** `/Patient/Appointments`

**Screenshot Description:**
Complete appointment history:
- Filter tabs (All, Upcoming, Past)
- Appointment cards showing:
  - Doctor name and specialty
  - Date and time
  - Location
  - Status badge
  - Reason for visit
  - **For Completed Appointments:**
    - Consultation notes (if added)
    - Prescription (if added)
    - "Leave Feedback" button (if not already left)
  - **For Future Appointments:**
    - Cancel button

---

### 4.22 Leave Feedback (Patient)
**File:** `Views/Patient/LeaveFeedback.cshtml`  
**Route:** `/Patient/LeaveFeedback/{appointmentId}`

**Screenshot Description:**
Feedback submission form:
- **Appointment Details (Read-only):**
  - Doctor name
  - Specialty
  - Appointment date
- **Feedback Form:**
  - Star rating selector (1-5 stars, clickable)
  - Comments textarea (optional)
  - Character counter
  - Submit Feedback button

**Validation:**
- Rating is required (1-5)
- Can only submit feedback once per appointment
- Appointment must be completed
- Comments are optional but limited to 1000 characters

**Implementation:**
```csharp
[HttpPost]
public async Task<IActionResult> LeaveFeedback(int appointmentId, int rating, string? comments)
{
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
```

---

### 4.23 My Feedbacks (Patient)
**File:** `Views/Patient/MyFeedbacks.cshtml`  
**Route:** `/Patient/MyFeedbacks`

**Screenshot Description:**
Patient's feedback history:
- List of feedbacks submitted:
  - Doctor name and specialty
  - Appointment date
  - Star rating (displayed visually)
  - Comments
  - Submission date

---

### 4.24 Payment History (Patient)
**File:** `Views/Patient/PaymentHistory.cshtml`  
**Route:** `/Patient/PaymentHistory`

**Screenshot Description:**
Complete payment transaction history for the patient:
- **Summary Card:**
  - Total amount paid (all successful payments)
- **Transactions Table:**
  - Payment ID
  - Doctor name and specialty
  - Appointment date
  - Amount in LKR
  - Payment status (color-coded badges)
  - Payment date and time
  - Transaction details
- **Statistics Cards:**
  - Total transactions count
  - Successful payments count (green)
  - Pending payments count (yellow)

**Features:**
- All payment transactions listed in reverse chronological order
- Color-coded status badges:
  - Paid (Green)
  - Pending (Yellow)
  - Failed (Red)
  - Refunded (Blue)
- Quick navigation to Dashboard and Appointments

**Implementation:**
```csharp
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
```

---

## 5. Database Implementation

### 5.1 Database Schema

The system uses 8 main database tables:

1. **AspNetUsers** - User authentication (Identity framework)
2. **AspNetRoles** - User roles (Admin, Doctor, Patient)
3. **AspNetUserRoles** - User-role relationships
4. **Doctors** - Doctor profiles and information
5. **Patients** - Patient profiles and medical information
6. **Specialties** - Medical specialties
7. **Appointments** - Appointment bookings
8. **Payments** - Payment transactions
9. **Feedbacks** - Patient feedback and ratings

### 5.2 Entity Relationships

```
ApplicationUser (1) ─────── (1) Doctor
ApplicationUser (1) ─────── (1) Patient
Specialty (1) ─────────── (N) Doctor
Doctor (1) ────────────── (N) Appointment
Patient (1) ───────────── (N) Appointment
Appointment (1) ─────────── (1) Payment
Doctor (1) ────────────── (N) Feedback
Patient (1) ───────────── (N) Feedback
Appointment (1) ────────── (1) Feedback (optional)
```

### 5.3 Database Models

#### Doctor Model
```csharp
public class Doctor
{
    public int Id { get; set; }
    [Required] public string UserId { get; set; }
    [Required] public int SpecialtyId { get; set; }
    [StringLength(200)] public string? Qualifications { get; set; }
    [StringLength(500)] public string? Bio { get; set; }
    [Required] [Range(0, 100000)] public decimal ConsultationFee { get; set; }
    [StringLength(200)] public string? Location { get; set; }
    public bool IsAvailable { get; set; } = true;
    public string? WorkingHours { get; set; }
    
    public ApplicationUser? User { get; set; }
    public Specialty? Specialty { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}
```

#### Appointment Model
```csharp
public class Appointment
{
    public int Id { get; set; }
    [Required] public int PatientId { get; set; }
    [Required] public int DoctorId { get; set; }
    [Required] public DateTime AppointmentDate { get; set; }
    [Required] [StringLength(50)] public string TimeSlot { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    [StringLength(500)] public string? Reason { get; set; }
    [StringLength(2000)] public string? ConsultationNotes { get; set; }
    [StringLength(2000)] public string? Prescription { get; set; }
    public int? PaymentId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    public Payment? Payment { get; set; }
}
```

#### Feedback Model
```csharp
public class Feedback
{
    public int Id { get; set; }
    [Required] public int PatientId { get; set; }
    [Required] public int DoctorId { get; set; }
    public int? AppointmentId { get; set; }
    [Required] [Range(1, 5)] public int Rating { get; set; }
    [StringLength(1000)] public string? Comments { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    public Appointment? Appointment { get; set; }
}
```

### 5.4 Sample Data

The database is seeded with:
- 1 Admin user: admin@healthcare.com
- 8 Medical specialties (General Medicine, Cardiology, Pediatrics, etc.)
- Sample doctors across different specialties
- Sample patient accounts
- Sample appointments and payments

---

## 6. Security Features

### 6.1 Authentication
- ASP.NET Core Identity for secure user management
- Password hashing using industry-standard algorithms
- Secure password requirements:
  - Minimum 6 characters
  - At least one digit required

### 6.2 Authorization
- Role-based access control (RBAC)
- [Authorize] attributes on controllers and actions
- Restricted routes based on user roles
- Prevents unauthorized access to admin/doctor features

### 6.3 Data Protection
- SQL injection prevention through Entity Framework parameterized queries
- Cross-Site Request Forgery (CSRF) protection via anti-forgery tokens
- Model validation to prevent invalid data entry
- Secure session management

### 6.4 Business Logic Security
- Patients can only view/cancel their own appointments
- Doctors can only manage their own appointments
- Admins have full system access
- Feedback can only be left once per appointment
- Appointments can only be booked for future dates

---

## 7. Code Structure

### 7.1 Project Organization

```
HealthcareAppointmentSystem/
├── Controllers/
│   ├── AccountController.cs       # Authentication
│   ├── AdminController.cs         # Admin functionality
│   ├── DoctorController.cs        # Doctor functionality
│   ├── PatientController.cs       # Patient functionality
│   └── HomeController.cs          # Public pages
├── Models/
│   ├── ApplicationUser.cs         # User entity
│   ├── Doctor.cs                  # Doctor entity
│   ├── Patient.cs                 # Patient entity
│   ├── Appointment.cs             # Appointment entity
│   ├── Payment.cs                 # Payment entity
│   ├── Specialty.cs               # Specialty entity
│   └── Feedback.cs                # Feedback entity
├── Views/
│   ├── Account/                   # Login/Register views
│   ├── Admin/                     # Admin views
│   ├── Doctor/                    # Doctor views
│   ├── Patient/                   # Patient views
│   ├── Home/                      # Public views
│   └── Shared/                    # Layout and partials
├── Data/
│   └── ApplicationDbContext.cs    # EF Core DbContext
├── wwwroot/
│   ├── css/                       # Stylesheets
│   ├── js/                        # JavaScript files
│   └── lib/                       # Third-party libraries
├── Program.cs                     # Application entry point
└── appsettings.json              # Configuration
```

### 7.2 Key Design Patterns

**MVC Pattern:**
- Models: Data entities and business logic
- Views: Razor templates for UI
- Controllers: Handle HTTP requests and responses

**Repository Pattern:**
- DbContext acts as repository
- Separation of data access from business logic

**Dependency Injection:**
- Services registered in Program.cs
- Injected into controllers via constructor

### 7.3 Database Context Configuration

```csharp
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships with cascade/restrict delete behaviors
        // Seed initial data (admin user, roles, specialties)
    }
}
```

---

## 8. Conclusion

The Healthcare Appointment System has been successfully implemented with all core features functioning as specified in the requirements. The system provides:

- ✓ Complete admin portal for system management
- ✓ Doctor portal for appointment and patient management
- ✓ Patient portal for searching doctors and booking appointments
- ✓ Comprehensive reporting system
- ✓ Feedback and rating system
- ✓ Secure authentication and authorization
- ✓ Responsive, user-friendly interface

### Future Enhancements
- Online payment integration with Stripe
- Email notifications for appointment confirmations
- SMS reminders for upcoming appointments
- Advanced search filters
- Doctor availability calendar
- Video consultation feature
- Mobile application
- Multi-language support

**Developed by:** Healthcare Development Team  
**Date:** October 26, 2025  
**Version:** 1.0
