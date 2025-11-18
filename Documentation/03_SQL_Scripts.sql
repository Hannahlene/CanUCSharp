-- ============================================
-- Healthcare Appointment System
-- SQL Scripts for Database Implementation
-- Database: SQL Server / SQLite Compatible
-- ============================================

-- ============================================
-- SECTION 1: TABLE CREATION (DDL)
-- ============================================

-- Drop existing tables if they exist (in correct order to avoid FK conflicts)
DROP TABLE IF EXISTS Feedbacks;
DROP TABLE IF EXISTS Payments;
DROP TABLE IF EXISTS Appointments;
DROP TABLE IF EXISTS Doctors;
DROP TABLE IF EXISTS Patients;
DROP TABLE IF EXISTS Specialties;
DROP TABLE IF EXISTS AspNetUserRoles;
DROP TABLE IF EXISTS AspNetRoles;
DROP TABLE IF EXISTS AspNetUsers;

-- Create AspNetUsers Table (Identity User)
CREATE TABLE AspNetUsers (
    Id TEXT PRIMARY KEY NOT NULL,
    UserName TEXT(256) UNIQUE NOT NULL,
    NormalizedUserName TEXT(256) UNIQUE,
    Email TEXT(256) UNIQUE NOT NULL,
    NormalizedEmail TEXT(256),
    EmailConfirmed INTEGER NOT NULL DEFAULT 0,
    PasswordHash TEXT,
    SecurityStamp TEXT,
    ConcurrencyStamp TEXT,
    PhoneNumber TEXT,
    PhoneNumberConfirmed INTEGER NOT NULL DEFAULT 0,
    TwoFactorEnabled INTEGER NOT NULL DEFAULT 0,
    LockoutEnd TEXT,
    LockoutEnabled INTEGER NOT NULL DEFAULT 1,
    AccessFailedCount INTEGER NOT NULL DEFAULT 0,
    FirstName TEXT,
    LastName TEXT,
    Address TEXT,
    DateRegistered TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Create AspNetRoles Table
CREATE TABLE AspNetRoles (
    Id TEXT PRIMARY KEY NOT NULL,
    Name TEXT(256) UNIQUE,
    NormalizedName TEXT(256) UNIQUE,
    ConcurrencyStamp TEXT
);

-- Create AspNetUserRoles Table (Junction Table)
CREATE TABLE AspNetUserRoles (
    UserId TEXT NOT NULL,
    RoleId TEXT NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);

-- Create Specialties Table
CREATE TABLE Specialties (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT(100) NOT NULL UNIQUE,
    Description TEXT
);

-- Create Doctors Table
CREATE TABLE Doctors (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId TEXT NOT NULL UNIQUE,
    SpecialtyId INTEGER NOT NULL,
    Qualifications TEXT(200),
    Bio TEXT(500),
    ConsultationFee DECIMAL(18,2) NOT NULL CHECK (ConsultationFee >= 0 AND ConsultationFee <= 100000),
    Location TEXT(200),
    IsAvailable INTEGER NOT NULL DEFAULT 1,
    WorkingHours TEXT,
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    FOREIGN KEY (SpecialtyId) REFERENCES Specialties(Id) ON DELETE RESTRICT
);

-- Create Patients Table
CREATE TABLE Patients (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId TEXT NOT NULL UNIQUE,
    DateOfBirth TEXT NOT NULL,
    Gender TEXT(10),
    EmergencyContact TEXT(200),
    MedicalHistory TEXT(1000),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

-- Create Appointments Table
CREATE TABLE Appointments (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId INTEGER NOT NULL,
    DoctorId INTEGER NOT NULL,
    AppointmentDate TEXT NOT NULL,
    TimeSlot TEXT(50) NOT NULL,
    Status INTEGER NOT NULL DEFAULT 0,
    Reason TEXT(500),
    ConsultationNotes TEXT(2000),
    Prescription TEXT(2000),
    PaymentId INTEGER,
    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE RESTRICT,
    FOREIGN KEY (DoctorId) REFERENCES Doctors(Id) ON DELETE RESTRICT
);

-- Create Payments Table
CREATE TABLE Payments (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    AppointmentId INTEGER NOT NULL UNIQUE,
    Amount DECIMAL(18,2) NOT NULL CHECK (Amount >= 0 AND Amount <= 100000),
    Status INTEGER NOT NULL DEFAULT 0,
    StripePaymentIntentId TEXT(100),
    PaymentDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    TransactionDetails TEXT(500),
    FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id) ON DELETE CASCADE
);

-- Create Feedbacks Table
CREATE TABLE Feedbacks (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId INTEGER NOT NULL,
    DoctorId INTEGER NOT NULL,
    AppointmentId INTEGER,
    Rating INTEGER NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
    Comments TEXT(1000),
    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE RESTRICT,
    FOREIGN KEY (DoctorId) REFERENCES Doctors(Id) ON DELETE RESTRICT,
    FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id) ON DELETE SET NULL
);

-- Create Indexes for Performance
CREATE INDEX IX_Doctors_UserId ON Doctors(UserId);
CREATE INDEX IX_Doctors_SpecialtyId ON Doctors(SpecialtyId);
CREATE INDEX IX_Patients_UserId ON Patients(UserId);
CREATE INDEX IX_Appointments_PatientId ON Appointments(PatientId);
CREATE INDEX IX_Appointments_DoctorId ON Appointments(DoctorId);
CREATE INDEX IX_Appointments_AppointmentDate ON Appointments(AppointmentDate);
CREATE INDEX IX_Payments_AppointmentId ON Payments(AppointmentId);
CREATE INDEX IX_Feedbacks_PatientId ON Feedbacks(PatientId);
CREATE INDEX IX_Feedbacks_DoctorId ON Feedbacks(DoctorId);

-- ============================================
-- SECTION 2: DATA INSERTION (DML)
-- ============================================

-- Insert Roles
INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES 
('1', 'Admin', 'ADMIN', 'admin-stamp-001'),
('2', 'Doctor', 'DOCTOR', 'doctor-stamp-001'),
('3', 'Patient', 'PATIENT', 'patient-stamp-001');

-- Insert Users (Sample Data)
-- Note: PasswordHash is hashed version of 'Admin123!' for admin, 'Doctor123!' for doctors, 'Patient123!' for patients
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, FirstName, LastName, PhoneNumber, Address, DateRegistered) VALUES 
('admin-001', 'admin@healthcare.com', 'ADMIN@HEALTHCARE.COM', 'admin@healthcare.com', 'ADMIN@HEALTHCARE.COM', 1, 'AQAAAAIAAYagAAAAEKxqLPm8H+...', 'System', 'Administrator', '0771234567', 'Colombo 07, Sri Lanka', '2025-01-01 08:00:00'),
('doctor-001', 'dr.silva@healthcare.com', 'DR.SILVA@HEALTHCARE.COM', 'dr.silva@healthcare.com', 'DR.SILVA@HEALTHCARE.COM', 1, 'AQAAAAIAAYagAAAAEKxqLPm8H+...', 'Amal', 'Silva', '0771234568', 'Kandy, Sri Lanka', '2025-01-02 09:00:00'),
('doctor-002', 'dr.perera@healthcare.com', 'DR.PERERA@HEALTHCARE.COM', 'dr.perera@healthcare.com', 'DR.PERERA@HEALTHCARE.COM', 1, 'AQAAAAIAAYagAAAAEKxqLPm8H+...', 'Nimal', 'Perera', '0771234569', 'Galle, Sri Lanka', '2025-01-02 09:30:00'),
('doctor-003', 'dr.fernando@healthcare.com', 'DR.FERNANDO@HEALTHCARE.COM', 'dr.fernando@healthcare.com', 'DR.FERNANDO@HEALTHCARE.COM', 1, 'AQAAAAIAAYagAAAAEKxqLPm8H+...', 'Kamani', 'Fernando', '0771234570', 'Colombo 03, Sri Lanka', '2025-01-02 10:00:00'),
('patient-001', 'kasun@email.com', 'KASUN@EMAIL.COM', 'kasun@email.com', 'KASUN@EMAIL.COM', 1, 'AQAAAAIAAYagAAAAEKxqLPm8H+...', 'Kasun', 'Rajapaksa', '0771234571', 'Nugegoda, Sri Lanka', '2025-01-03 10:00:00'),
('patient-002', 'saman@email.com', 'SAMAN@EMAIL.COM', 'saman@email.com', 'SAMAN@EMAIL.COM', 1, 'AQAAAAIAAYagAAAAEKxqLPm8H+...', 'Saman', 'Wickramasinghe', '0771234572', 'Dehiwala, Sri Lanka', '2025-01-03 11:00:00'),
('patient-003', 'malini@email.com', 'MALINI@EMAIL.COM', 'malini@email.com', 'MALINI@EMAIL.COM', 1, 'AQAAAAIAAYagAAAAEKxqLPm8H+...', 'Malini', 'Jayawardena', '0771234573', 'Moratuwa, Sri Lanka', '2025-01-03 12:00:00');

-- Assign User Roles
INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES 
('admin-001', '1'),
('doctor-001', '2'),
('doctor-002', '2'),
('doctor-003', '2'),
('patient-001', '3'),
('patient-002', '3'),
('patient-003', '3');

-- Insert Specialties
INSERT INTO Specialties (Name, Description) VALUES 
('General Medicine', 'Primary care and general health issues'),
('Pediatrics', 'Healthcare for infants, children, and adolescents'),
('Cardiology', 'Heart and cardiovascular system specialists'),
('Dermatology', 'Skin, hair, and nail conditions'),
('Orthopedics', 'Bone, joint, and muscle disorders'),
('Psychiatry', 'Mental health and behavioral disorders'),
('Gynecology', 'Women''s reproductive health'),
('Neurology', 'Brain and nervous system disorders');

-- Insert Doctors
INSERT INTO Doctors (UserId, SpecialtyId, Qualifications, Bio, ConsultationFee, Location, IsAvailable, WorkingHours) VALUES 
('doctor-001', 1, 'MBBS, MD (General Medicine)', 'Experienced general physician with 15 years of practice', 3500.00, 'MediCare Clinic, Kandy', 1, 'Mon-Fri: 9:00 AM - 5:00 PM'),
('doctor-002', 3, 'MBBS, MD (Cardiology), FRCP', 'Specialist in cardiovascular diseases and interventions', 5000.00, 'MediCare Heart Center, Galle', 1, 'Mon-Sat: 10:00 AM - 6:00 PM'),
('doctor-003', 2, 'MBBS, DCH (Pediatrics)', 'Dedicated to child healthcare and development', 3000.00, 'MediCare Children''s Clinic, Colombo', 1, 'Tue-Sat: 8:00 AM - 4:00 PM');

-- Insert Patients
INSERT INTO Patients (UserId, DateOfBirth, Gender, EmergencyContact, MedicalHistory) VALUES 
('patient-001', '1990-05-15', 'Male', 'Wife: 0771234580', 'Asthma, managed with inhaler'),
('patient-002', '1985-08-22', 'Male', 'Brother: 0771234581', 'Hypertension, on medication'),
('patient-003', '1995-03-10', 'Female', 'Mother: 0771234582', 'No significant medical history');

-- Insert Appointments
INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, TimeSlot, Status, Reason, ConsultationNotes, Prescription, CreatedAt) VALUES 
(1, 1, '2025-10-28', '10:00 AM - 11:00 AM', 1, 'Annual checkup', 'Vitals normal. Continue current medication.', 'Salbutamol inhaler - use as needed', '2025-10-26 08:30:00'),
(2, 2, '2025-10-29', '02:00 PM - 03:00 PM', 0, 'Chest pain consultation', NULL, NULL, '2025-10-26 09:15:00'),
(3, 3, '2025-10-30', '09:00 AM - 10:00 AM', 1, 'Child vaccination', NULL, NULL, '2025-10-26 10:00:00'),
(1, 2, '2025-11-05', '11:00 AM - 12:00 PM', 0, 'Follow-up cardiology', NULL, NULL, '2025-10-26 11:30:00');

-- Insert Payments
INSERT INTO Payments (AppointmentId, Amount, Status, PaymentDate, TransactionDetails) VALUES 
(1, 3500.00, 1, '2025-10-26 08:35:00', 'Online payment - Card ending 1234'),
(3, 3000.00, 1, '2025-10-26 10:05:00', 'Online payment - Card ending 5678');

-- Insert Feedbacks
INSERT INTO Feedbacks (PatientId, DoctorId, AppointmentId, Rating, Comments, CreatedAt) VALUES 
(1, 1, 1, 5, 'Excellent doctor! Very thorough and caring.', '2025-10-26 14:00:00'),
(3, 3, 3, 4, 'Good experience, but waiting time was a bit long.', '2025-10-26 15:30:00');

-- ============================================
-- SECTION 3: DATA RETRIEVAL (SELECT)
-- ============================================

-- Display all Specialties
SELECT * FROM Specialties ORDER BY Name;

-- Display all Doctors with their Specialty
SELECT 
    d.Id,
    u.FirstName || ' ' || u.LastName AS DoctorName,
    u.Email,
    s.Name AS Specialty,
    d.Qualifications,
    d.ConsultationFee,
    d.Location,
    d.IsAvailable,
    d.WorkingHours
FROM Doctors d
INNER JOIN AspNetUsers u ON d.UserId = u.Id
INNER JOIN Specialties s ON d.SpecialtyId = s.Id
ORDER BY s.Name, u.LastName;

-- Display all Patients
SELECT 
    p.Id,
    u.FirstName || ' ' || u.LastName AS PatientName,
    u.Email,
    u.PhoneNumber,
    p.DateOfBirth,
    p.Gender,
    p.EmergencyContact
FROM Patients p
INNER JOIN AspNetUsers u ON p.UserId = u.Id
ORDER BY u.LastName;

-- Display all Appointments with Details
SELECT 
    a.Id,
    pu.FirstName || ' ' || pu.LastName AS PatientName,
    du.FirstName || ' ' || du.LastName AS DoctorName,
    s.Name AS Specialty,
    a.AppointmentDate,
    a.TimeSlot,
    CASE a.Status
        WHEN 0 THEN 'Pending'
        WHEN 1 THEN 'Confirmed'
        WHEN 2 THEN 'Completed'
        WHEN 3 THEN 'Cancelled'
        WHEN 4 THEN 'Rescheduled'
    END AS Status,
    a.Reason,
    a.CreatedAt
FROM Appointments a
INNER JOIN Patients p ON a.PatientId = p.Id
INNER JOIN AspNetUsers pu ON p.UserId = pu.Id
INNER JOIN Doctors d ON a.DoctorId = d.Id
INNER JOIN AspNetUsers du ON d.UserId = du.Id
INNER JOIN Specialties s ON d.SpecialtyId = s.Id
ORDER BY a.AppointmentDate DESC, a.TimeSlot;

-- Display all Payments
SELECT 
    pay.Id,
    a.Id AS AppointmentId,
    pu.FirstName || ' ' || pu.LastName AS PatientName,
    du.FirstName || ' ' || du.LastName AS DoctorName,
    pay.Amount,
    CASE pay.Status
        WHEN 0 THEN 'Pending'
        WHEN 1 THEN 'Completed'
        WHEN 2 THEN 'Failed'
        WHEN 3 THEN 'Refunded'
    END AS PaymentStatus,
    pay.PaymentDate,
    pay.TransactionDetails
FROM Payments pay
INNER JOIN Appointments a ON pay.AppointmentId = a.Id
INNER JOIN Patients p ON a.PatientId = p.Id
INNER JOIN AspNetUsers pu ON p.UserId = pu.Id
INNER JOIN Doctors d ON a.DoctorId = d.Id
INNER JOIN AspNetUsers du ON d.UserId = du.Id
ORDER BY pay.PaymentDate DESC;

-- Display all Feedbacks
SELECT 
    f.Id,
    pu.FirstName || ' ' || pu.LastName AS PatientName,
    du.FirstName || ' ' || du.LastName AS DoctorName,
    f.Rating,
    f.Comments,
    f.CreatedAt
FROM Feedbacks f
INNER JOIN Patients p ON f.PatientId = p.Id
INNER JOIN AspNetUsers pu ON p.UserId = pu.Id
INNER JOIN Doctors d ON f.DoctorId = d.Id
INNER JOIN AspNetUsers du ON d.UserId = du.Id
ORDER BY f.CreatedAt DESC;

-- ============================================
-- SECTION 4: USEFUL QUERIES AND REPORTS
-- ============================================

-- Get Doctor Statistics
SELECT 
    du.FirstName || ' ' || du.LastName AS DoctorName,
    s.Name AS Specialty,
    COUNT(a.Id) AS TotalAppointments,
    SUM(CASE WHEN a.Status = 1 THEN 1 ELSE 0 END) AS ConfirmedAppointments,
    SUM(CASE WHEN a.Status = 2 THEN 1 ELSE 0 END) AS CompletedAppointments,
    AVG(CAST(f.Rating AS REAL)) AS AverageRating,
    COUNT(DISTINCT f.Id) AS TotalFeedbacks
FROM Doctors d
INNER JOIN AspNetUsers du ON d.UserId = du.Id
INNER JOIN Specialties s ON d.SpecialtyId = s.Id
LEFT JOIN Appointments a ON d.Id = a.DoctorId
LEFT JOIN Feedbacks f ON d.Id = f.DoctorId
GROUP BY d.Id, du.FirstName, du.LastName, s.Name
ORDER BY TotalAppointments DESC;

-- Get Patient Appointment History
SELECT 
    pu.FirstName || ' ' || pu.LastName AS PatientName,
    COUNT(a.Id) AS TotalAppointments,
    SUM(CASE WHEN a.Status = 2 THEN 1 ELSE 0 END) AS CompletedAppointments,
    SUM(CASE WHEN a.Status = 3 THEN 1 ELSE 0 END) AS CancelledAppointments,
    SUM(pay.Amount) AS TotalAmountPaid
FROM Patients p
INNER JOIN AspNetUsers pu ON p.UserId = pu.Id
LEFT JOIN Appointments a ON p.Id = a.PatientId
LEFT JOIN Payments pay ON a.Id = pay.AppointmentId AND pay.Status = 1
GROUP BY p.Id, pu.FirstName, pu.LastName
ORDER BY TotalAppointments DESC;

-- Get Upcoming Appointments (Next 7 Days)
SELECT 
    a.AppointmentDate,
    a.TimeSlot,
    pu.FirstName || ' ' || pu.LastName AS PatientName,
    pu.PhoneNumber AS PatientPhone,
    du.FirstName || ' ' || du.LastName AS DoctorName,
    s.Name AS Specialty,
    d.Location,
    CASE a.Status
        WHEN 0 THEN 'Pending'
        WHEN 1 THEN 'Confirmed'
    END AS Status
FROM Appointments a
INNER JOIN Patients p ON a.PatientId = p.Id
INNER JOIN AspNetUsers pu ON p.UserId = pu.Id
INNER JOIN Doctors d ON a.DoctorId = d.Id
INNER JOIN AspNetUsers du ON d.UserId = du.Id
INNER JOIN Specialties s ON d.SpecialtyId = s.Id
WHERE a.AppointmentDate >= date('now')
  AND a.AppointmentDate <= date('now', '+7 days')
  AND a.Status IN (0, 1)
ORDER BY a.AppointmentDate, a.TimeSlot;

-- Get Revenue Report by Specialty
SELECT 
    s.Name AS Specialty,
    COUNT(DISTINCT d.Id) AS NumberOfDoctors,
    COUNT(a.Id) AS TotalAppointments,
    SUM(CASE WHEN pay.Status = 1 THEN pay.Amount ELSE 0 END) AS TotalRevenue,
    AVG(CASE WHEN pay.Status = 1 THEN pay.Amount ELSE NULL END) AS AverageConsultationFee
FROM Specialties s
LEFT JOIN Doctors d ON s.Id = d.SpecialtyId
LEFT JOIN Appointments a ON d.Id = a.DoctorId
LEFT JOIN Payments pay ON a.Id = pay.AppointmentId
GROUP BY s.Id, s.Name
ORDER BY TotalRevenue DESC;

-- Get Top Rated Doctors
SELECT 
    du.FirstName || ' ' || du.LastName AS DoctorName,
    s.Name AS Specialty,
    d.Location,
    AVG(CAST(f.Rating AS REAL)) AS AverageRating,
    COUNT(f.Id) AS TotalReviews
FROM Doctors d
INNER JOIN AspNetUsers du ON d.UserId = du.Id
INNER JOIN Specialties s ON d.SpecialtyId = s.Id
LEFT JOIN Feedbacks f ON d.Id = f.DoctorId
GROUP BY d.Id, du.FirstName, du.LastName, s.Name, d.Location
HAVING COUNT(f.Id) > 0
ORDER BY AverageRating DESC, TotalReviews DESC
LIMIT 10;

-- ============================================
-- END OF SQL SCRIPTS
-- ============================================
