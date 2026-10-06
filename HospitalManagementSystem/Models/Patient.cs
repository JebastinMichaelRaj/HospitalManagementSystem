using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class Patient
    {
        [Key]
        public string? PatientID { get; set; } = Guid.NewGuid().ToString();
        public string? UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ContactNumber { get; set; }
        public DateTime? RegisteredAt { get; set; }

        // Navigation property
        public User? User { get; set; }
    }
}