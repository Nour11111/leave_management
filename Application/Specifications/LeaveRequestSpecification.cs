using LeaveManagement.Core.Enums;

namespace LeaveManagement.Application.Specifications
{
    public class LeaveRequestSpecification
    {
        public int? EmployeeId { get; set; }
        public LeaveType? LeaveType { get; set; }
        public LeaveStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string SortBy { get; set; }
        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;  // Default value
        public string ReasonKeyword { get; set; }

        
    }
}
