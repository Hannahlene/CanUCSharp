# Healthcare Appointment System

## Overview
A comprehensive C# ASP.NET Core MVC web application for managing healthcare appointments. The system provides role-based access for Administrators, Doctors, and Patients with features for appointment booking and medical record management.

## Technology Stack
- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQLite with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Frontend**: Razor Views with Bootstrap 5

## Project Structure
- **Models**: Database entities (ApplicationUser, Doctor, Patient, Appointment, Specialty)
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
- Book and confirm appointments
- View appointment history and medical records
- Cancel appointments

## Default Accounts
- **Admin**: admin@healthcare.com / Admin123!

## Database Models
- **ApplicationUser**: Extended IdentityUser with custom fields
- **Doctor**: Doctor profiles linked to users
- **Patient**: Patient profiles linked to users
- **Specialty**: Medical specialties
- **Appointment**: Appointment bookings with status tracking
- **Payment**: Payment transaction records
- **Feedback**: Patient feedback and ratings for doctors

## Recent Changes
- **October 26, 2025 (Latest)**: Completed coursework documentation and features
  - Added comprehensive Feedback/Rating system for patients
  - Implemented complete Admin Reporting system:
    - System statistics dashboard
    - Detailed appointment reports
    - Payment reports
    - Patient engagement statistics
  - Created complete coursework documentation:
    - Database ER Diagram documentation
    - Data Dictionary with all attributes and constraints
    - SQL Scripts (DDL, DML, SELECT queries)
    - Testing Documentation with 29 test cases
    - User Manual (3 pages) for all user roles
    - Implementation Documentation with functionality descriptions
  - Database recreated with Feedback table and proper relationships
  
- **October 26, 2025 (Earlier)**: Removed payment system (Stripe integration) per user request
  - Removed Stripe.net package from dependencies
  - Simplified appointment booking flow to auto-confirm without payment
  - Updated all patient views to remove payment UI
  - Fixed security vulnerability in CancelAppointment to prevent cross-patient cancellations
  
- Initial project setup with .NET 8.0
- Created all database models and relationships
- Implemented ASP.NET Identity for authentication
- Built Admin, Doctor, and Patient controllers
- Created all necessary Razor views

## Coursework Documentation
All documentation files are located in the `Documentation/` folder:
1. **01_Database_ER_Diagram.md** - Complete ER diagram and relationships
2. **02_Data_Dictionary.md** - Comprehensive data dictionary with constraints
3. **03_SQL_Scripts.sql** - Table creation, data insertion, and queries
4. **04_Testing_Documentation.md** - 29 test cases across all modules (89.7% pass rate)
5. **05_User_Manual.md** - User guide for Admin, Doctor, and Patient roles
6. **06_Implementation_Documentation.md** - Detailed implementation documentation

## Notes
- Database seeding creates default admin account and sample specialties
- Appointments are automatically confirmed upon booking (no payment required)
- The application uses SQLite for easy development and deployment
- Patients can only cancel their own appointments (ownership verification implemented)
- Feedback can only be left for completed appointments (one feedback per appointment)
- Admin has access to comprehensive reports for system monitoring
