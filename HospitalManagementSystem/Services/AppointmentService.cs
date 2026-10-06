using HospitalManagementSystem.Data;
using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            // Includes the connected Patient and Doctor data for display
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(string doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorID == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(string patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientID == patientId)
                .ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(string appointmentId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentID == appointmentId);
        }

        public async Task CreateAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointmentStatusAsync(string appointmentId, string newStatus)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment != null)
            {
                appointment.Status = newStatus;
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
            }
        }
    }
}