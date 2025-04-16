using LeaveManagement.Application.Specifications;
using LeaveManagement.Core.Entities;

namespace LeaveManagement.Application.Interfaces.Repositories
{
    public interface ILeaveRequestRepository

    {
        Task<List<LeaveRequest>> GetAllAsync();
        Task<List<LeaveRequest>> GetAllAsync(LeaveRequestSpecification specification);

        Task<LeaveRequest> GetByIdAsync(int id);
        Task<List<LeaveRequest>> GetByEmployeeIdAsync(int employeeId); 
        Task AddAsync(LeaveRequest leaveRequest);
        Task UpdateAsync(LeaveRequest leaveRequest);
        Task DeleteAsync(LeaveRequest leaveRequest);
    }
}
