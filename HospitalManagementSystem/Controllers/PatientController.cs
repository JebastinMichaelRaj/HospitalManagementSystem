using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{
    // Restrict this entire controller so only logged-in users can access it
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        // Inject the service we created earlier
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: View all patients (Restricted to Admins and Doctors)
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return View(patients);
        }

        // GET: View a specific patient's details
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: /Patient/Edit/5 (Returns Partial View for the Modal)
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();

            // Pass the existing patient data to the modal
            return PartialView("_Edit", patient);
        }

        // POST: /Patient/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                await _patientService.UpdatePatientProfileAsync(patient);
                return Ok(new { message = "Patient details updated successfully!" });
            }

            // If validation fails (e.g. missing First Name), tell JS to keep modal open
            Response.StatusCode = 400;
            return PartialView("_Edit", patient);
        }
    }
}