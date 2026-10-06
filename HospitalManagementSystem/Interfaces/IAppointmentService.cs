using HospitalManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(string doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(string patientId);
        Task<Appointment> GetAppointmentByIdAsync(string appointmentId);
        Task CreateAppointmentAsync(Appointment appointment);
        Task UpdateAppointmentStatusAsync(string appointmentId, string newStatus);
    }
}