# Healthcare Appointment System - Entity Relationship Diagram

## Database Schema Overview

This document describes the complete Entity Relationship (ER) diagram for the Healthcare Appointment System database.

## ER Diagram Description

```
┌─────────────────────┐
│  AspNetUsers        │
│  (Identity)         │
├─────────────────────┤
│ Id (PK)             │
│ UserName            │
│ Email               │
│ PhoneNumber         │
│ FirstName           │
│ LastName            │
│ Address             │
│ DateRegistered      │
└──────────┬──────────┘
           │
           │ 1:1
           │
    ┌──────┴──────┐
    │             │
    │             │
┌───▼──────────┐  │
│  Doctor      │  │
├──────────────┤  │
│ Id (PK)      │  │
│ UserId (FK)  │  │
│ SpecialtyId  │  │
│ Qualifications│  │
│ Bio          │  │
│ ConsultationFee│ │
│ Location     │  │
│ IsAvailable  │  │
│ WorkingHours │  │
└──────┬───────┘  │
       │          │
       │ N:1      │
       │          │
┌──────▼──────┐   │
│  Specialty  │   │
├─────────────┤   │
│ Id (PK)     │   │
│ Name        │   │
│ Description │   │
└─────────────┘   │
                  │
              ┌───▼──────────┐
              │  Patient     │
              ├──────────────┤
              │ Id (PK)      │
              │ UserId (FK)  │
              │ DateOfBirth  │
              │ Gender       │
              │ EmergencyContact│
              │ MedicalHistory  │
              └──────┬───────┘
                     │
                     │
        ┌────────────┴────────────┐
        │                         │
        │ N                     N │
        │                         │
┌───────▼─────────┐       ┌───────▼─────────┐
│  Appointment    │       │  Feedback       │
├─────────────────┤       ├─────────────────┤
│ Id (PK)         │       │ Id (PK)         │
│ PatientId (FK)  │       │ PatientId (FK)  │
│ DoctorId (FK)   │       │ DoctorId (FK)   │
│ AppointmentDate │       │ Rating          │
│ TimeSlot        │       │ Comments        │
│ Status          │       │ CreatedAt       │
│ Reason          │       └─────────────────┘
│ ConsultationNotes│
│ Prescription    │
│ PaymentId (FK)  │
│ CreatedAt       │
└────────┬────────┘
         │
         │ 1:1
         │
┌────────▼────────┐
│  Payment        │
├─────────────────┤
│ Id (PK)         │
│ AppointmentId(FK)│
│ Amount          │
│ Status          │
│ StripePaymentIntentId│
│ PaymentDate     │
│ TransactionDetails│
└─────────────────┘
```

## Entity Relationships

### 1. **ApplicationUser (AspNetUsers)**
- Central entity inherited from ASP.NET Core Identity
- **Relationships:**
  - 1:1 with Doctor (One user can be one doctor)
  - 1:1 with Patient (One user can be one patient)

### 2. **Doctor**
- **Primary Key:** Id
- **Foreign Keys:** 
  - UserId → AspNetUsers.Id (CASCADE DELETE)
  - SpecialtyId → Specialty.Id
- **Relationships:**
  - N:1 with Specialty (Many doctors can have one specialty)
  - 1:N with Appointment (One doctor can have many appointments)
  - 1:N with Feedback (One doctor can receive many feedbacks)

### 3. **Patient**
- **Primary Key:** Id
- **Foreign Keys:** 
  - UserId → AspNetUsers.Id (CASCADE DELETE)
- **Relationships:**
  - 1:N with Appointment (One patient can have many appointments)
  - 1:N with Feedback (One patient can leave many feedbacks)

### 4. **Specialty**
- **Primary Key:** Id
- **Relationships:**
  - 1:N with Doctor (One specialty can have many doctors)

### 5. **Appointment**
- **Primary Key:** Id
- **Foreign Keys:** 
  - PatientId → Patient.Id (RESTRICT DELETE)
  - DoctorId → Doctor.Id (RESTRICT DELETE)
  - PaymentId → Payment.Id (nullable)
- **Relationships:**
  - N:1 with Patient (Many appointments belong to one patient)
  - N:1 with Doctor (Many appointments belong to one doctor)
  - 1:1 with Payment (One appointment has one payment)

### 6. **Payment**
- **Primary Key:** Id
- **Foreign Keys:** 
  - AppointmentId → Appointment.Id (CASCADE DELETE)
- **Relationships:**
  - 1:1 with Appointment (One payment belongs to one appointment)

### 7. **Feedback**
- **Primary Key:** Id
- **Foreign Keys:** 
  - PatientId → Patient.Id (RESTRICT DELETE)
  - DoctorId → Doctor.Id (RESTRICT DELETE)
- **Relationships:**
  - N:1 with Patient (Many feedbacks from one patient)
  - N:1 with Doctor (Many feedbacks for one doctor)

## Cardinality Summary

| Relationship | From | To | Type |
|--------------|------|-----|------|
| User → Doctor | ApplicationUser | Doctor | 1:1 |
| User → Patient | ApplicationUser | Patient | 1:1 |
| Specialty → Doctor | Specialty | Doctor | 1:N |
| Doctor → Appointment | Doctor | Appointment | 1:N |
| Patient → Appointment | Patient | Appointment | 1:N |
| Appointment → Payment | Appointment | Payment | 1:1 |
| Doctor → Feedback | Doctor | Feedback | 1:N |
| Patient → Feedback | Patient | Feedback | 1:N |

## Database Constraints

### Delete Behaviors
- **CASCADE**: User deletion cascades to Doctor/Patient and Payment deletion
- **RESTRICT**: Cannot delete Doctor/Patient if they have appointments or feedback

### Data Integrity Rules
1. Every Doctor must have a valid User and Specialty
2. Every Patient must have a valid User
3. Every Appointment must have valid Patient and Doctor
4. Payment is optional for Appointment
5. All monetary values must be between 0 and 100,000
6. Status enums enforce valid state transitions
