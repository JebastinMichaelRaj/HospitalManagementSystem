using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    public class Doctor
    {
        [Key]
        public string DoctorID { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey("User")]
        public string? UserID { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Specialty { get; set; }
        public string? Department { get; set; }

        // Navigation properties
        public User? User { get; set; } // One-to-One relationship with User
        public ICollection<Appointment>? Appointments { get; set; } // One-to-Many relationship with Appointments
    }
}