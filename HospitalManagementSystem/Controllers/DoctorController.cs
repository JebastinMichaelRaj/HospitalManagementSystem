using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{
    [Authorize]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // GET: /Doctor/Index — All authenticated users can see the doctor directory
        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return View(doctors);
        }

        // GET: /Doctor/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // GET: /Doctor/Create — Returns Partial View for the Modal (Admin only)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return PartialView("_Create", new Doctor());
        }

        // POST: /Doctor/Create — Saves a new doctor profile (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                await _doctorService.CreateDoctorAsync(doctor);
                return Ok(new { message = "Doctor added successfully!" });
            }
            return PartialView("_Create", doctor);
        }

        // GET: /Doctor/Edit/5 — Returns Partial View for the Modal
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();

            return PartialView("_Edit", doctor);
        }

        // POST: /Doctor/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> Edit(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                await _doctorService.UpdateDoctorProfileAsync(doctor);
                return Ok(new { message = "Doctor profile updated successfully!" });
            }
            return PartialView("_Edit", doctor);
        }
    }
}