using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    public class Appointment
    {
        [Key]
        public string AppointmentID { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey("Patient")]
        public string? PatientID { get; set; }

        [ForeignKey("Doctor")]
        public string? DoctorID { get; set; }

        public DateTime? AppointmentDate { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public Billing? Billing { get; set; } // One-to-One relationship with Billing
    }
}