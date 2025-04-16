using AutoMapper;
using LeaveManagement.Application.Interfaces.Repositories;
using LeaveManagement.Core.Entities;

namespace LeaveManagement.Application.Queries
{
    public class GetAllLeaveRequestsQuery
    {
    }

    // Query Handler Class: GetAllLeaveRequestsHandler
    public class GetAllLeaveRequestsHandler
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILogger<GetAllLeaveRequestsHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllLeaveRequestsHandler(
            ILeaveRequestRepository leaveRequestRepository,
            ILogger<GetAllLeaveRequestsHandler> logger,
            IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<LeaveRequest>> HandleAsync(GetAllLeaveRequestsQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all leave requests");
            var leaveRequests = await _leaveRequestRepository.GetAllAsync();
            if (leaveRequests == null)
            {
                _logger.LogWarning("No leave requests found.");
                return new List<LeaveRequest>(); // Return an empty list, not null
            }

            // Map the entity to the DTO
            var leaveRequestDtos = _mapper.Map<List<LeaveRequest>>(leaveRequests);
            return leaveRequestDtos;
        }
    }

}
