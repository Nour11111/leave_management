using AutoMapper;
using LeaveManagement.Application.Interfaces.Repositories;
using LeaveManagement.Core.Entities;

namespace LeaveManagement.Application.Queries
{
    public class GetLeaveRequestByIdQuery
    {
        public int Id { get; set; }
    }

    // Query Handler Class: GetLeaveRequestByIdHandler
    public class GetLeaveRequestByIdHandler
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILogger<GetLeaveRequestByIdHandler> _logger;
        private readonly IMapper _mapper;

        public GetLeaveRequestByIdHandler(
            ILeaveRequestRepository leaveRequestRepository,
            ILogger<GetLeaveRequestByIdHandler> logger,
            IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<LeaveRequest> HandleAsync(GetLeaveRequestByIdQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting leave request by ID: {LeaveRequestId}", query.Id);
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(query.Id);
            if (leaveRequest == null)
            {
                _logger.LogWarning("LeaveRequest with ID {LeaveRequestId} not found.", query.Id);
                return null;
            }
            // Map the entity to the DTO
            return leaveRequest;
        }
    }
}
