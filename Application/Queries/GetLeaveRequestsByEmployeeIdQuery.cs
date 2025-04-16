using AutoMapper;
using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.Interfaces.Repositories;

namespace LeaveManagement.Application.Queries
{
    public class GetLeaveRequestsByEmployeeIdQuery
    {
        public int EmployeeId { get; set; }
    }

    // Query Handler Class: GetLeaveRequestsByEmployeeIdHandler
    public class GetLeaveRequestsByEmployeeIdHandler
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILogger<GetLeaveRequestsByEmployeeIdHandler> _logger;
        private readonly IMapper _mapper;

        public GetLeaveRequestsByEmployeeIdHandler(
            ILeaveRequestRepository leaveRequestRepository,
            ILogger<GetLeaveRequestsByEmployeeIdHandler> logger,
            IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<LeaveRequestByEmployeeIdDto>> HandleAsync(GetLeaveRequestsByEmployeeIdQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting leave requests for employee ID: {EmployeeId}", query.EmployeeId);
            var leaveRequests = await _leaveRequestRepository.GetByEmployeeIdAsync(query.EmployeeId);
            if (leaveRequests == null)
            {
                _logger.LogWarning("LeaveRequest for Employee ID {EmployeeId} not found.", query.EmployeeId);
                return null;
            }
            // Map the entity to the DTO
            var leaveRequestDtos = _mapper.Map<List<LeaveRequestByEmployeeIdDto>>(leaveRequests);
            return leaveRequestDtos;
        }
    }

}
