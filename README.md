# Healthcare Appointment System

A comprehensive ASP.NET Core 8.0 MVC web application for managing healthcare appointments with role-based access control.

## Features

### Admin Portal
- Add, edit, and delete doctor profiles
- Manage medical specialties
- View all registered patients and doctors
- Monitor system statistics (total doctors, patients, appointments)

### Doctor Portal
- Update profile and availability settings
- View upcoming appointments
- Confirm, reschedule, or cancel appointments
- Add consultation notes and prescriptions for patients

### Patient Portal
- Register and create account
- Search for doctors by specialty and location
- Book appointments with preferred doctors
- Pay consultation fees online via Stripe
- View appointment history and medical records

## Technology Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQLite with Entity Framework Core
- **Authentication**: ASP.NET Core Identity with role-based authorization
- **Payment**: Stripe.net SDK for online payments
- **Frontend**: Razor Views with Bootstrap 5

## Getting Started

### Prerequisites
- .NET 8.0 SDK

### Running the Application

The application is already configured and running. Simply access it through your browser.

### Default Login Credentials

**Administrator Account:**
- Email: `admin@healthcare.com`
- Password: `Admin123!`

Use this admin account to:
1. Add doctors to the system
2. Manage specialties
3. View all patients and appointments

## Database

The application uses SQLite for data storage. The database is automatically created and seeded with:
- Three user roles (Admin, Doctor, Patient)
- Five medical specialties (General Medicine, Cardiology, Dermatology, Pediatrics, Orthopedics)
- Default admin account

## Payment Integration

The application includes Stripe integration for processing consultation fees. To enable live payments:

1. Get your Stripe API keys from [Stripe Dashboard](https://dashboard.stripe.com/)
2. Update `appsettings.json` with your keys:
   ```json
   "Stripe": {
     "PublishableKey": "your_publishable_key",
     "SecretKey": "your_secret_key"
   }
   ```

## Project Structure

```
├── Controllers/          # MVC Controllers (Admin, Doctor, Patient, Account, Home)
├── Models/              # Database entities
│   ├── ApplicationUser.cs
│   ├── Doctor.cs
│   ├── Patient.cs
│   ├── Appointment.cs
│   ├── Specialty.cs
│   └── Payment.cs
├── Views/               # Razor view templates
│   ├── Admin/          # Admin views
│   ├── Doctor/         # Doctor views
│   ├── Patient/        # Patient views
│   ├── Account/        # Login/Register views
│   └── Shared/         # Shared layout
├── Data/                # Database context
│   └── ApplicationDbContext.cs
└── wwwroot/             # Static files (CSS, JS, images)
```

## User Workflows

### For Patients
1. Register for an account
2. Search for doctors by specialty or location
3. Book an appointment
4. Pay consultation fee via Stripe
5. View appointment confirmation and history

### For Doctors
1. Sign in with doctor credentials (created by admin)
2. Update profile and working hours
3. View upcoming appointments
4. Confirm or cancel appointments
5. Add consultation notes and prescriptions

### For Administrators
1. Sign in with admin credentials
2. Add new doctors with specialties
3. Manage specialties
4. View all registered users
5. Monitor system statistics

## Security Features

- Password hashing with ASP.NET Identity
- Role-based authorization on all sensitive actions
- CSRF protection on forms
- Secure payment processing via Stripe

## Future Enhancements

- Email notifications for appointment confirmations
- Advanced reporting and analytics
- Patient feedback system
- Appointment reminders via SMS
- Medical records file upload
- Advanced search filters (doctor ratings, availability calendar)
