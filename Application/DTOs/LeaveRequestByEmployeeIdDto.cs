using LeaveManagement.Core.Enums;

namespace LeaveManagement.Application.DTOs
{
    public class LeaveRequestByEmployeeIdDto
    {
        public int Id { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; }
    }
}
