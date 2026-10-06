using HospitalManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Interfaces
{
    public interface IPatientService
    {
        // Define the contract for patient operations
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(string patientId);
        Task<Patient> GetPatientByUserIdAsync(string userId);
        Task UpdatePatientProfileAsync(Patient patient);
    }
}