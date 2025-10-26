# Healthcare Appointment System

## Overview
A comprehensive C# ASP.NET Core MVC web application for managing healthcare appointments. The system provides role-based access for Administrators, Doctors, and Patients with features for appointment booking, payment processing, and medical record management.

## Technology Stack
- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQLite with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Payment**: Stripe.net SDK
- **Frontend**: Razor Views with Bootstrap 5

## Project Structure
- **Models**: Database entities (ApplicationUser, Doctor, Patient, Appointment, Specialty, Payment)
- **Controllers**: Business logic for Admin, Doctor, Patient, Account, and Home
- **Views**: Razor view templates organized by controller
- **Data**: ApplicationDbContext for database operations

## Key Features

### Admin Portal
- Manage doctor profiles and specialties
- View all registered patients and doctors
- Add/edit/delete doctors and specialties
- Monitor system statistics

### Doctor Portal
- Update personal profile and availability
- View upcoming appointments
- Confirm, reschedule, or cancel appointments
- Add consultation notes and prescriptions

### Patient Portal
- Search for doctors by specialty and location
- Book and reserve appointments
- Pay consultation fees via Stripe
- View appointment history and medical records

## Default Accounts
- **Admin**: admin@healthcare.com / Admin123!

## Database Models
- **ApplicationUser**: Extended IdentityUser with custom fields
- **Doctor**: Doctor profiles linked to users
- **Patient**: Patient profiles linked to users
- **Specialty**: Medical specialties
- **Appointment**: Appointment bookings with status tracking
- **Payment**: Payment records with Stripe integration

## Recent Changes
- Initial project setup with .NET 8.0
- Created all database models and relationships
- Implemented ASP.NET Identity for authentication
- Built Admin, Doctor, and Patient controllers
- Created all necessary Razor views
- Integrated Stripe for payment processing

## Notes
- Database seeding creates default admin account and sample specialties
- Stripe integration requires valid API keys in appsettings.json
- The application uses SQLite for easy development and deployment
