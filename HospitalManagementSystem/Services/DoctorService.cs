using HospitalManagementSystem.Data;
using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext _context;

        public DoctorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.Doctors
                .Include(d => d.User)
                .ToListAsync();
        }

        public async Task<Doctor> GetDoctorByIdAsync(string doctorId)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.DoctorID == doctorId);
        }

        public async Task<Doctor> GetDoctorByUserIdAsync(string userId)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.UserID == userId);
        }

        public async Task UpdateDoctorProfileAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task CreateDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }
    }
}