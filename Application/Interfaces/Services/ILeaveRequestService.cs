using LeaveManagement.Core.Entities;

namespace LeaveManagement.Application.Interfaces.Services
{
    public interface ILeaveRequestService
    {
        Task ValidateLeaveRequest(LeaveRequest leaveRequest);
    }
}
