using HospitalManagementSystem.Data;
using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        // Inject the database context via the constructor
        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            // Fetch all patients and include their connected User account data
            return await _context.Patients
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(string patientId)
        {
            return await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PatientID == patientId);
        }

        public async Task<Patient> GetPatientByUserIdAsync(string userId)
        {
            return await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserID == userId);
        }

        public async Task UpdatePatientProfileAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }
    }
}