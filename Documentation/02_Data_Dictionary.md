# Healthcare Appointment System - Data Dictionary

## Complete Attribute List and Constraints

---

## 1. AspNetUsers (ApplicationUser)

**Description:** Stores user authentication and profile information. Extends ASP.NET Core Identity.

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| Id | string | PRIMARY KEY, NOT NULL | Unique identifier for user |
| UserName | string(256) | UNIQUE, NOT NULL | Username for login |
| NormalizedUserName | string(256) | UNIQUE, INDEXED | Normalized username for searching |
| Email | string(256) | UNIQUE, NOT NULL | User email address |
| NormalizedEmail | string(256) | UNIQUE, INDEXED | Normalized email for searching |
| EmailConfirmed | bit | NOT NULL, DEFAULT 0 | Email verification status |
| PasswordHash | string | NULL | Hashed password |
| SecurityStamp | string | NULL | Security stamp for password changes |
| ConcurrencyStamp | string | NULL | Concurrency token |
| PhoneNumber | string | NULL | Contact phone number |
| PhoneNumberConfirmed | bit | NOT NULL, DEFAULT 0 | Phone verification status |
| TwoFactorEnabled | bit | NOT NULL, DEFAULT 0 | Two-factor authentication status |
| LockoutEnd | datetimeoffset | NULL | Account lockout expiry time |
| LockoutEnabled | bit | NOT NULL, DEFAULT 1 | Whether lockout is enabled |
| AccessFailedCount | int | NOT NULL, DEFAULT 0 | Failed login attempts counter |
| FirstName | string | NULL | User's first name |
| LastName | string | NULL | User's last name |
| Address | string | NULL | Physical address |
| DateRegistered | datetime | NOT NULL, DEFAULT GETUTCDATE() | Registration timestamp |

**Indexes:**
- PRIMARY KEY on Id
- UNIQUE INDEX on NormalizedUserName
- UNIQUE INDEX on NormalizedEmail

---

## 2. Doctors

**Description:** Stores doctor-specific information and credentials.

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| Id | int | PRIMARY KEY, IDENTITY(1,1) | Unique doctor identifier |
| UserId | string | FOREIGN KEY, NOT NULL, UNIQUE | Reference to AspNetUsers.Id |
| SpecialtyId | int | FOREIGN KEY, NOT NULL | Reference to Specialty.Id |
| Qualifications | string(200) | NULL | Doctor's qualifications/degrees |
| Bio | string(500) | NULL | Professional biography |
| ConsultationFee | decimal(18,2) | NOT NULL, CHECK (>= 0 AND <= 100000) | Fee per consultation |
| Location | string(200) | NULL | Clinic/hospital location |
| IsAvailable | bit | NOT NULL, DEFAULT 1 | Current availability status |
| WorkingHours | string | NULL | Schedule information |

**Constraints:**
- FOREIGN KEY UserId REFERENCES AspNetUsers(Id) ON DELETE CASCADE
- FOREIGN KEY SpecialtyId REFERENCES Specialties(Id) ON DELETE NO ACTION
- CHECK ConsultationFee BETWEEN 0 AND 100000

**Indexes:**
- PRIMARY KEY on Id
- UNIQUE INDEX on UserId
- INDEX on SpecialtyId

---

## 3. Patients

**Description:** Stores patient medical and personal information.

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| Id | int | PRIMARY KEY, IDENTITY(1,1) | Unique patient identifier |
| UserId | string | FOREIGN KEY, NOT NULL, UNIQUE | Reference to AspNetUsers.Id |
| DateOfBirth | datetime | NOT NULL | Patient's birth date |
| Gender | string(10) | NULL | Patient's gender |
| EmergencyContact | string(200) | NULL | Emergency contact details |
| MedicalHistory | string(1000) | NULL | Medical history notes |

**Constraints:**
- FOREIGN KEY UserId REFERENCES AspNetUsers(Id) ON DELETE CASCADE

**Indexes:**
- PRIMARY KEY on Id
- UNIQUE INDEX on UserId

---

## 4. Specialties

**Description:** Medical specialty categories for doctors.

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| Id | int | PRIMARY KEY, IDENTITY(1,1) | Unique specialty identifier |
| Name | string(100) | NOT NULL, UNIQUE | Specialty name |
| Description | string | NULL | Specialty description |

**Constraints:**
- UNIQUE constraint on Name

**Indexes:**
- PRIMARY KEY on Id
- UNIQUE INDEX on Name

---

## 5. Appointments

**Description:** Stores appointment bookings and consultation details.

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| Id | int | PRIMARY KEY, IDENTITY(1,1) | Unique appointment identifier |
| PatientId | int | FOREIGN KEY, NOT NULL | Reference to Patients.Id |
| DoctorId | int | FOREIGN KEY, NOT NULL | Reference to Doctors.Id |
| AppointmentDate | datetime | NOT NULL | Date of appointment |
| TimeSlot | string(50) | NOT NULL | Time slot (e.g., "09:00-10:00") |
| Status | int | NOT NULL, DEFAULT 0 | Status enum (0-4) |
| Reason | string(500) | NULL | Reason for visit |
| ConsultationNotes | string(2000) | NULL | Doctor's consultation notes |
| Prescription | string(2000) | NULL | Prescribed medications |
| PaymentId | int | FOREIGN KEY, NULL | Reference to Payments.Id |
| CreatedAt | datetime | NOT NULL, DEFAULT GETUTCDATE() | Booking timestamp |

**Status Enum Values:**
- 0 = Pending
- 1 = Confirmed
- 2 = Completed
- 3 = Cancelled
- 4 = Rescheduled

**Constraints:**
- FOREIGN KEY PatientId REFERENCES Patients(Id) ON DELETE RESTRICT
- FOREIGN KEY DoctorId REFERENCES Doctors(Id) ON DELETE RESTRICT
- FOREIGN KEY PaymentId REFERENCES Payments(Id) ON DELETE NO ACTION

**Indexes:**
- PRIMARY KEY on Id
- INDEX on PatientId
- INDEX on DoctorId
- INDEX on AppointmentDate

---

## 6. Payments

**Description:** Stores payment transaction information.

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| Id | int | PRIMARY KEY, IDENTITY(1,1) | Unique payment identifier |
| AppointmentId | int | FOREIGN KEY, NOT NULL, UNIQUE | Reference to Appointments.Id |
| Amount | decimal(18,2) | NOT NULL, CHECK (>= 0 AND <= 100000) | Payment amount |
| Status | int | NOT NULL, DEFAULT 0 | Payment status enum (0-3) |
| StripePaymentIntentId | string(100) | NULL | Stripe transaction ID |
| PaymentDate | datetime | NOT NULL, DEFAULT GETUTCDATE() | Payment timestamp |
| TransactionDetails | string(500) | NULL | Additional transaction info |

**Status Enum Values:**
- 0 = Pending
- 1 = Completed
- 2 = Failed
- 3 = Refunded

**Constraints:**
- FOREIGN KEY AppointmentId REFERENCES Appointments(Id) ON DELETE CASCADE
- UNIQUE constraint on AppointmentId
- CHECK Amount BETWEEN 0 AND 100000

**Indexes:**
- PRIMARY KEY on Id
- UNIQUE INDEX on AppointmentId

---

## 7. Feedbacks (To Be Implemented)

**Description:** Stores patient feedback and ratings for doctors.

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| Id | int | PRIMARY KEY, IDENTITY(1,1) | Unique feedback identifier |
| PatientId | int | FOREIGN KEY, NOT NULL | Reference to Patients.Id |
| DoctorId | int | FOREIGN KEY, NOT NULL | Reference to Doctors.Id |
| AppointmentId | int | FOREIGN KEY, NULL | Reference to Appointments.Id |
| Rating | int | NOT NULL, CHECK (>= 1 AND <= 5) | Rating (1-5 stars) |
| Comments | string(1000) | NULL | Feedback comments |
| CreatedAt | datetime | NOT NULL, DEFAULT GETUTCDATE() | Feedback timestamp |

**Constraints:**
- FOREIGN KEY PatientId REFERENCES Patients(Id) ON DELETE RESTRICT
- FOREIGN KEY DoctorId REFERENCES Doctors(Id) ON DELETE RESTRICT
- FOREIGN KEY AppointmentId REFERENCES Appointments(Id) ON DELETE SET NULL
- CHECK Rating BETWEEN 1 AND 5

**Indexes:**
- PRIMARY KEY on Id
- INDEX on PatientId
- INDEX on DoctorId
- INDEX on AppointmentDate

---

## Database Relationships Summary

### One-to-One Relationships
1. AspNetUsers ↔ Doctors (via UserId)
2. AspNetUsers ↔ Patients (via UserId)

### One-to-Many Relationships
1. Specialties → Doctors (One specialty has many doctors)
2. Doctors → Appointments (One doctor has many appointments)
3. Patients → Appointments (One patient has many appointments)
4. Doctors → Feedbacks (One doctor receives many feedbacks)
5. Patients → Feedbacks (One patient can leave many feedbacks)

### Referential Integrity Rules
- **CASCADE DELETE**: User deletion removes associated Doctor/Patient records
- **RESTRICT DELETE**: Cannot delete Doctor/Patient with existing appointments
- **SET NULL**: Deleting appointment sets feedback's AppointmentId to NULL

---

## Data Validation Rules

### String Length Limits
- Email: 256 characters
- Names: 100-200 characters depending on field
- Notes/Comments: 500-2000 characters
- Descriptions: Variable length text


### Date Constraints
- All timestamps use UTC
- DateOfBirth must be in the past
- AppointmentDate should be in the future for new bookings

### Enum Constraints
- AppointmentStatus: {Pending, Confirmed, Completed, Cancelled, Rescheduled}
