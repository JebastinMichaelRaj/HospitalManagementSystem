using HospitalManagementSystem.Data;
using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{
    // Restrict access to logged-in users only
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly ApplicationDbContext _context;

        // Inject all necessary services
        public AppointmentController(
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            IPatientService patientService,
            ApplicationDbContext context)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _patientService = patientService;
            _context = context;
        }

        // GET: View Appointments based on User Role
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            // Admins see everything
            if (userRole == "Admin")
            {
                var allAppointments = await _appointmentService.GetAllAppointmentsAsync();
                return View(allAppointments);
            }
            // Doctors only see their own schedule
            else if (userRole == "Doctor")
            {
                var doctor = await _doctorService.GetDoctorByUserIdAsync(userId);
                if (doctor == null)
                {
                    TempData["Error"] = "Doctor profile not found. Please contact the administrator.";
                    return RedirectToAction("Index", "Home");
                }
                var docAppointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(doctor.DoctorID);
                return View(docAppointments);
            }
            // Patients only see their own appointments
            else
            {
                var patient = await _patientService.GetPatientByUserIdAsync(userId);
                if (patient == null)
                {
                    TempData["Error"] = "Patient profile not found. Please contact the administrator.";
                    return RedirectToAction("Index", "Home");
                }
                var patAppointments = await _appointmentService.GetAppointmentsByPatientIdAsync(patient.PatientID);
                return View(patAppointments);
            }
        }

        // GET: Show the Booking Form (Restricted to Patients and Admins)
        [Authorize(Roles = "Admin, Patient")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Doctors = await _doctorService.GetAllDoctorsAsync();
            return PartialView("_Create", new Appointment());
        }

        // POST: Process the Booking
        [HttpPost]
        [Authorize(Roles = "Admin, Patient")]
        public async Task<IActionResult> Create(string doctorId, DateTime appointmentDate, string notes)
        {
            // Step 1: Prevent past-date booking
            if (appointmentDate < DateTime.Now)
            {
                ViewBag.Error = "Appointment date cannot be in the past. Please select a future date and time.";
                ViewBag.Doctors = await _doctorService.GetAllDoctorsAsync();
                return PartialView("_Create", new Appointment());
            }

            // Step 2: Prevent double-booking (within 30-minute window)
            var existingAppointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId);
            bool isDoubleBooked = existingAppointments
                .Any(a => a.AppointmentDate.HasValue &&
                          Math.Abs((a.AppointmentDate.Value - appointmentDate).TotalMinutes) < 30 &&
                          a.Status != "Cancelled");

            if (isDoubleBooked)
            {
                ViewBag.Error = "The selected doctor already has an appointment within 30 minutes of this time slot. Please choose another time.";
                ViewBag.Doctors = await _doctorService.GetAllDoctorsAsync();
                return PartialView("_Create", new Appointment());
            }

            // Step 3: Get the patient's ID based on the logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var patient = await _patientService.GetPatientByUserIdAsync(userId);

            if (patient == null)
            {
                ViewBag.Error = "Patient profile not found. Please update your profile first.";
                ViewBag.Doctors = await _doctorService.GetAllDoctorsAsync();
                return PartialView("_Create", new Appointment());
            }

            // Step 4: Create the appointment record
            var newAppointment = new Appointment
            {
                PatientID = patient.PatientID,
                DoctorID = doctorId,
                AppointmentDate = appointmentDate,
                Status = "Scheduled",
                Notes = notes
            };

            await _appointmentService.CreateAppointmentAsync(newAppointment);

            // Step 5: Signal success to JavaScript modal handler
            Response.Headers["X-Success-Message"] = "Appointment booked successfully!";
            return Ok(new { message = "Appointment booked successfully!" });
        }

        // GET: /Appointment/Edit/5 (For Doctors and Admins to update status)
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();

            return PartialView("_Edit", appointment);
        }

        // POST: /Appointment/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> Edit(Appointment appointment)
        {
            // Remove Navigation properties from validation
            ModelState.Remove("Patient");
            ModelState.Remove("Doctor");
            ModelState.Remove("Billing");

            if (ModelState.IsValid)
            {
                // Update the appointment status
                await _appointmentService.UpdateAppointmentStatusAsync(appointment.AppointmentID, appointment.Status);

                // Auto-generate a billing record when doctor marks appointment as Completed
                if (appointment.Status == "Completed")
                {
                    bool billExists = _context.Billings.Any(b => b.AppointmentID == appointment.AppointmentID);

                    if (!billExists)
                    {
                        var newInvoice = new Billing
                        {
                            BillingID = Guid.NewGuid().ToString(),
                            AppointmentID = appointment.AppointmentID,
                            TotalAmount = 150.00m, // Standard default consultation fee
                            PaymentStatus = "Pending",
                            BillingDate = DateTime.Now
                        };

                        _context.Billings.Add(newInvoice);
                        await _context.SaveChangesAsync();
                    }
                }

                return Ok(new { message = "Appointment status updated successfully!" });
            }

            Response.StatusCode = 400;
            return PartialView("_Edit", appointment);
        }

        // POST: Cancel an appointment (Patient can cancel their own)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(string id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();

            // Patients can only cancel their own appointments
            if (userRole == "Patient")
            {
                var patient = await _patientService.GetPatientByUserIdAsync(userId);
                if (patient == null || appointment.PatientID != patient.PatientID)
                {
                    TempData["Error"] = "You are not authorized to cancel this appointment.";
                    return RedirectToAction(nameof(Index));
                }
            }

            await _appointmentService.UpdateAppointmentStatusAsync(id, "Cancelled");
            TempData["Success"] = "Appointment cancelled successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}