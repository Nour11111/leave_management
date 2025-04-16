using FluentValidation;
using LeaveManagement.Application.Interfaces.Repositories;
using LeaveManagement.Core.Entities;
using LeaveManagement.Core.Enums;
using AutoMapper;
using LeaveManagement.Application.Services;
using LeaveManagement.Application.Interfaces.Services;

namespace LeaveManagement.Application.Commands
{
    // 1. Command Class

    // 1. Command Class
    public class CreateLeaveRequestCommand
    {
        public int EmployeeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }

    // 2. Command Handler Class
    public class CreateLeaveRequestCommandHandler
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IValidator<CreateLeaveRequestCommand> _validator;
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILogger<CreateLeaveRequestCommandHandler> _logger;
        private readonly IMapper _mapper;  

        public CreateLeaveRequestCommandHandler(
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveRequestService leaveRequestService,
            IValidator<CreateLeaveRequestCommand> validator,
            ILogger<CreateLeaveRequestCommandHandler> logger,
            IMapper mapper) 
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _leaveRequestService = leaveRequestService ?? throw new ArgumentNullException(nameof(leaveRequestService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> HandleAsync(CreateLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            // Input validation
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateLeaveRequestCommand: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            // Map the command to the entity using AutoMapper
            var leaveRequest = _mapper.Map<LeaveRequest>(command);
            leaveRequest.Status = LeaveStatus.Pending; // Set the initial status
            leaveRequest.CreatedAt = DateTime.UtcNow;


            // Implement Business Rule Validation
            await _leaveRequestService.ValidateLeaveRequest(leaveRequest);

            // Add to the database using the repository
            await _leaveRequestRepository.AddAsync(leaveRequest);
            _logger.LogInformation("LeaveRequest created with ID: {LeaveRequestId}", leaveRequest.Id);

            // Return the new leave request's ID
            return leaveRequest.Id;
        }
    }

    // 3. Command Validator
    public class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
    {
        public CreateLeaveRequestCommandValidator()
        {
            RuleFor(c => c.EmployeeId)
                .NotEmpty().WithMessage("EmployeeId is required.");

            RuleFor(c => c.LeaveType)
                .IsInEnum().WithMessage("LeaveType is invalid.");

            RuleFor(c => c.StartDate)
                .NotEmpty().WithMessage("StartDate is required.")
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("StartDate must be today or in the future.");

            RuleFor(c => c.EndDate)
                .NotEmpty().WithMessage("EndDate is required.")
                .GreaterThanOrEqualTo(c => c.StartDate.Date).WithMessage("EndDate must be on or after StartDate.");

            RuleFor(c => c.Reason)
                .NotEmpty().WithMessage("Reason is required.")
                .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.");
        }
    }
}