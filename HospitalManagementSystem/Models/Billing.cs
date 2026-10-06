using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema; // 1. ADD THIS NAMESPACE

namespace HospitalManagementSystem.Models
{
    // 2. ADD THIS TAG TO FORCE EF CORE TO USE THE SINGULAR TABLE NAME
    [Table("Billing")]
    public class Billing
    {
        public string? BillingID { get; set; }
        public string? AppointmentID { get; set; }
        public decimal TotalAmount { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? BillingDate { get; set; }

        [ValidateNever]
        public Appointment? Appointment { get; set; }
    }
}