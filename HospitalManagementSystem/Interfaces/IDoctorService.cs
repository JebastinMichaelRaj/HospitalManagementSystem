using HospitalManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        Task<Doctor> GetDoctorByIdAsync(string doctorId);
        Task<Doctor> GetDoctorByUserIdAsync(string userId);
        Task UpdateDoctorProfileAsync(Doctor doctor);
        Task CreateDoctorAsync(Doctor doctor);
    }
}