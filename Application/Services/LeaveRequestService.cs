using LeaveManagement.Application.Exceptions;
using LeaveManagement.Application.Interfaces.Repositories;
using LeaveManagement.Application.Interfaces.Services;
using LeaveManagement.Core.Entities;
using LeaveManagement.Core.Enums;

namespace LeaveManagement.Application.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
        }

        public async Task ValidateLeaveRequest(LeaveRequest leaveRequest)
        {
            // No overlapping leave dates per employee
            var overlappingRequests = await _leaveRequestRepository.GetByEmployeeIdAsync(leaveRequest.EmployeeId)
                .ConfigureAwait(false);
            if (overlappingRequests.Any(lr => lr.StartDate < leaveRequest.EndDate && lr.EndDate > leaveRequest.StartDate && lr.Id != leaveRequest.Id)) // Exclude the current leave request in update scenario
            {
                throw new OverlappingLeaveException($"Overlapping leave dates detected for employee {leaveRequest.EmployeeId}.");
            }

            // Max 20 annual leave days per year
            if (leaveRequest.LeaveType == LeaveType.Annual)
            {
                var annualLeaveDaysThisYear = overlappingRequests
                    .Where(lr => lr.LeaveType == LeaveType.Annual && lr.StartDate.Year == leaveRequest.StartDate.Year && lr.Id != leaveRequest.Id)
                    .Sum(lr => (lr.EndDate - lr.StartDate).Days);
                if ((leaveRequest.EndDate - leaveRequest.StartDate).Days + annualLeaveDaysThisYear > 20)
                {
                    throw new MaxAnnualLeaveDaysExceededException($"Maximum 20 annual leave days per year exceeded for employee {leaveRequest.EmployeeId}.");
                }
            }

            // Sick leave requires a non-empty reason
            if (leaveRequest.LeaveType == LeaveType.Sick && string.IsNullOrEmpty(leaveRequest.Reason))
            {
                throw new SickLeaveReasonRequiredException($"Sick leave request for employee {leaveRequest.EmployeeId} requires a non-empty reason.");
            }
        }
    }
}
