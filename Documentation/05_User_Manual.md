# Healthcare Appointment System - User Manual

**Version 1.0**  
**MediCare Pvt Ltd**  
**October 2025**

---

## Table of Contents

1. [Introduction](#1-introduction)
2. [System Access](#2-system-access)
3. [Admin User Guide](#3-admin-user-guide)
4. [Doctor User Guide](#4-doctor-user-guide)
5. [Patient User Guide](#5-patient-user-guide)
6. [Troubleshooting](#6-troubleshooting)

---

## 1. Introduction

The Healthcare Appointment System is a web-based platform designed to streamline appointment management for MediCare clinics and medical centers. The system supports three user roles:

- **Administrators** - Manage doctors, specialties, and system settings
- **Doctors** - Manage appointments and patient consultations
- **Patients** - Search for doctors and book appointments

### System Requirements
- Modern web browser (Chrome, Firefox, Safari, or Edge)
- Internet connection
- Screen resolution: 1024x768 or higher recommended

---

## 2. System Access

### Accessing the System
1. Open your web browser
2. Navigate to the Healthcare Appointment System URL
3. You will see the home page with sign-in options

### First-Time User Registration (Patients Only)
1. Click the **"Register"** button on the home page
2. Fill in the registration form:
   - **Email Address** - Your valid email (will be your username)
   - **Password** - Must be at least 6 characters
   - **First Name** and **Last Name**
   - **Address** (optional)
3. Click **"Register"** to create your account
4. You will be automatically logged in after successful registration

### Signing In
1. Click **"Sign In"** on the home page
2. Enter your **Email Address** and **Password**
3. Click the **"Sign In"** button
4. You will be redirected to your role-specific dashboard

### Signing Out
- Click your name in the top-right corner
- Select **"Logout"** from the dropdown menu

---

## 3. Admin User Guide

**Default Admin Credentials:**
- Email: admin@healthcare.com
- Password: Admin123!

### 3.1 Admin Dashboard

After signing in, you'll see the Admin Dashboard displaying:
- Total number of doctors in the system
- Total number of registered patients
- Total number of appointments
- Quick access menu to all admin functions

### 3.2 Managing Doctors

#### Viewing All Doctors
1. From the Admin menu, click **"Doctors"**
2. You'll see a list of all registered doctors with their:
   - Name and contact information
   - Medical specialty
   - Consultation fee
   - Location
   - Availability status

#### Adding a New Doctor
1. Click **"Add Doctor"** button
2. Fill in the required information:
   - **Email Address** - Doctor's email (will be their login)
   - **Password** - Initial password for the doctor
   - **First Name** and **Last Name**
   - **Specialty** - Select from dropdown
   - **Qualifications** - e.g., "MBBS, MD"
   - **Bio** - Brief professional description
   - **Consultation Fee** - Amount in LKR
   - **Location** - Clinic/hospital location
   - **Working Hours** - e.g., "Mon-Fri: 9AM-5PM"
3. Click **"Add Doctor"** to save
4. The doctor can now log in with the provided credentials

#### Editing Doctor Information
1. In the doctors list, find the doctor you want to edit
2. Click the **"Edit"** button next to their name
3. Update the necessary information
4. Click **"Update"** to save changes

#### Deleting a Doctor
1. Find the doctor in the list
2. Click the **"Delete"** button
3. Confirm the deletion when prompted
4. **Warning:** This will permanently remove the doctor and their associated user account

### 3.3 Managing Specialties

#### Viewing Specialties
1. Click **"Specialties"** in the Admin menu
2. View the list of all medical specialties

#### Adding a Specialty
1. Scroll to the "Add New Specialty" form
2. Enter:
   - **Name** - Specialty name (e.g., "Cardiology")
   - **Description** - Brief description
3. Click **"Add Specialty"**

#### Deleting a Specialty
1. Find the specialty in the list
2. Click **"Delete"**
3. **Note:** You cannot delete a specialty that has doctors assigned to it

### 3.4 Viewing Patients
1. Click **"Patients"** in the Admin menu
2. View all registered patients with their:
   - Name and contact information
   - Date of birth
   - Gender
   - Emergency contact
   - Medical history

#### Patient Statistics
1. Click **"Patient Statistics"**
2. View patient engagement metrics:
   - Total appointments per patient
   - Completed vs cancelled appointments
   - Total amount spent

---

## 4. Doctor User Guide

### 4.1 Doctor Dashboard

After signing in, you'll see:
- Your profile information
- Upcoming appointments for the day/week
- Quick statistics

### 4.2 Viewing Appointments

1. Click **"Appointments"** in the Doctor menu
2. You'll see all your appointments with:
   - Patient name and contact
   - Appointment date and time slot
   - Reason for visit
   - Current status

### 4.3 Managing Appointments

#### Confirming an Appointment
1. Find the pending appointment
2. Click **"Confirm"** button
3. The status will change to "Confirmed"

#### Cancelling an Appointment
1. Find the appointment to cancel
2. Click **"Cancel"** button
3. Confirm the cancellation
4. Status changes to "Cancelled"

### 4.4 Adding Consultation Notes

1. Click on a completed appointment
2. In the appointment details:
   - Enter **Consultation Notes** - Your observations and diagnosis
   - Enter **Prescription** - Medications and instructions
3. Click **"Save"** to record the information
4. Patients can view these notes in their appointment history

### 4.5 Updating Your Profile

1. Click **"Update Profile"** in the Doctor menu
2. You can modify:
   - Bio/Professional description
   - Working hours
   - Availability status (toggle on/off)
3. Click **"Update Profile"** to save changes
4. **Note:** Admin manages your consultation fee and specialty

---

## 5. Patient User Guide

### 5.1 Patient Dashboard

After signing in, you'll see:
- Your upcoming appointments
- Quick links to search doctors and view history

### 5.2 Searching for Doctors

1. Click **"Search Doctors"** in the Patient menu
2. Use search filters:
   - **Specialty** - Select from dropdown (optional)
   - **Location** - Enter city or area (optional)
3. Click **"Search"**
4. Browse the results showing:
   - Doctor name and qualifications
   - Specialty and location
   - Consultation fee
   - Working hours

### 5.3 Booking an Appointment

1. From the search results, click **"Book Appointment"** for your chosen doctor
2. Fill in the booking form:
   - **Appointment Date** - Select a future date
   - **Time Slot** - Enter preferred time (e.g., "10:00 AM - 11:00 AM")
   - **Reason** - Brief description of your health concern
3. Click **"Book Appointment"**
4. You'll see a confirmation message
5. Your appointment will be in "Pending" status until the doctor confirms it

### 5.4 Viewing Your Appointments

1. Click **"My Appointments"** in the Patient menu
2. See all your appointments (past and upcoming) with:
   - Doctor name and specialty
   - Date and time
   - Status
   - Consultation notes (for completed appointments)
   - Prescription (for completed appointments)

### 5.5 Cancelling an Appointment

1. In "My Appointments", find the appointment you want to cancel
2. Click **"Cancel"** button
3. Confirm the cancellation
4. **Note:** You can only cancel future appointments


## 6. Troubleshooting

### Cannot Log In
- **Check your credentials:** Ensure email and password are correct
- **Reset password:** Contact admin for password reset
- **Account not activated:** For new patients, ensure you completed registration

### Cannot Book Appointment
- **Check date:** Appointment date must be in the future
- **Doctor availability:** Ensure the doctor is marked as available
- **Login required:** You must be signed in as a patient

### Appointment Not Showing
- **Refresh the page:** Click the browser refresh button
- **Check status filter:** Ensure you're viewing the correct status (All, Upcoming, Past)
- **Verify user role:** Confirm you're logged in with the correct account

### Cannot Leave Feedback
- **Appointment must be completed:** Only completed appointments can receive feedback
- **Already submitted:** You can only leave feedback once per appointment
- **Ownership:** You can only leave feedback for your own appointments

### General Issues
1. **Clear browser cache:** Go to browser settings and clear browsing data
2. **Try a different browser:** Test with Chrome, Firefox, or Edge
3. **Contact support:** Reach out to system administrator for technical issues

### System Performance
- If pages load slowly, check your internet connection
- Close unnecessary browser tabs
- Ensure you're using an up-to-date browser version

---

## Getting Help

For additional assistance:
- **Technical Support:** Contact your system administrator
- **Medical Emergencies:** Call emergency services (1990 in Sri Lanka)
- **Clinic Information:** Contact your nearest MediCare location directly

**End of User Manual**
