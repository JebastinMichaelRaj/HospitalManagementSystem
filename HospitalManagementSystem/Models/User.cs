using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class User
    {
        [Key]
        public string UserID { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}