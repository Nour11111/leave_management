using AutoMapper;
using LeaveManagement.Application.Interfaces.Repositories;
using LeaveManagement.Application.Specifications;
using LeaveManagement.Core.Entities;

namespace LeaveManagement.Application.Queries
{
    
    public class GetLeaveRequestsWithFilterQuery
    {
        public LeaveRequestSpecification Specification { get; set; }
    }

    public class GetLeaveRequestsWithFilterHandler
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILogger<GetLeaveRequestsWithFilterHandler> _logger;
        private readonly IMapper _mapper;

        public GetLeaveRequestsWithFilterHandler(
            ILeaveRequestRepository leaveRequestRepository,
            ILogger<GetLeaveRequestsWithFilterHandler> logger,
            IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<LeaveRequest>> HandleAsync(GetLeaveRequestsWithFilterQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetLeaveRequestsWithFilterQuery");

            // Call the repository with the specification
            return await _leaveRequestRepository.GetAllAsync(query.Specification);


        }
    }
}
