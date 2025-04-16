using LeaveManagement.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Core.Entities
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }


        [Required]
        public LeaveType LeaveType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public LeaveStatus Status { get; set; }

        [Required]
        [StringLength(500)] // Add validation for Reason
        public string Reason { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    }
}
