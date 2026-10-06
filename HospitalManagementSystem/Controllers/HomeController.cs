using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly ApplicationDbContext _context;

        public HomeController(
            IAppointmentService appointmentService,
            IPatientService patientService,
            IDoctorService doctorService,
            ApplicationDbContext context)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // 1. If the user is NOT logged in, show them the public landing page
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            // 2. If they ARE logged in, identify their role and route to their dashboard
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userRole == "Admin")
            {
                var patients = await _patientService.GetAllPatientsAsync();
                var appointments = await _appointmentService.GetAllAppointmentsAsync();
                var doctors = await _doctorService.GetAllDoctorsAsync();
                var pendingBillings = await _context.Billings
                    .Where(b => b.PaymentStatus == "Pending")
                    .CountAsync();

                ViewBag.TotalPatients = patients.Count();
                ViewBag.TotalAppointments = appointments.Count();
                ViewBag.TotalDoctors = doctors.Count();
                ViewBag.PendingBillings = pendingBillings;
                ViewBag.TodayAppointments = appointments
                    .Count(a => a.AppointmentDate.HasValue &&
                                a.AppointmentDate.Value.Date == System.DateTime.Today);

                return View("AdminDashboard");
            }
            else if (userRole == "Doctor")
            {
                var doctor = await _doctorService.GetDoctorByUserIdAsync(userId);
                if (doctor == null)
                {
                    TempData["Error"] = "Your doctor profile is incomplete. Please contact the administrator.";
                    return View();
                }

                var appointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(doctor.DoctorID);

                ViewBag.DoctorName = doctor.FirstName ?? "Doctor";
                ViewBag.DoctorLastName = doctor.LastName ?? "";
                ViewBag.DoctorSpecialty = doctor.Specialty ?? "";
                ViewBag.UpcomingAppointments = appointments.Count(a => a.Status == "Scheduled");
                ViewBag.CompletedAppointments = appointments.Count(a => a.Status == "Completed");
                ViewBag.TodayAppointments = appointments
                    .Count(a => a.AppointmentDate.HasValue &&
                                a.AppointmentDate.Value.Date == System.DateTime.Today &&
                                a.Status == "Scheduled");

                return View("DoctorDashboard");
            }
            else // Patient
            {
                var patient = await _patientService.GetPatientByUserIdAsync(userId);
                if (patient == null)
                {
                    TempData["Error"] = "Your patient profile is incomplete. Please contact the administrator.";
                    return View();
                }

                var appointments = await _appointmentService.GetAppointmentsByPatientIdAsync(patient.PatientID);
                var nextAppointment = appointments
                    .Where(a => a.AppointmentDate.HasValue &&
                                a.AppointmentDate.Value > System.DateTime.Now &&
                                a.Status == "Scheduled")
                    .OrderBy(a => a.AppointmentDate)
                    .FirstOrDefault();

                ViewBag.PatientName = patient.FirstName ?? "Patient";
                ViewBag.MyAppointments = appointments.Count();
                ViewBag.UpcomingAppointments = appointments.Count(a => a.Status == "Scheduled");
                ViewBag.CompletedAppointments = appointments.Count(a => a.Status == "Completed");
                ViewBag.NextAppointmentDate = nextAppointment?.AppointmentDate?.ToString("MMM dd, yyyy hh:mm tt") ?? "None scheduled";
                ViewBag.NextAppointmentDoctor = nextAppointment?.Doctor != null
                    ? $"Dr. {nextAppointment.Doctor.FirstName} {nextAppointment.Doctor.LastName}"
                    : "";

                return View("PatientDashboard");
            }
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}