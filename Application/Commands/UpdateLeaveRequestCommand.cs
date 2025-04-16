using AutoMapper;
using FluentValidation;
using LeaveManagement.Application.Interfaces.Repositories;
using LeaveManagement.Application.Interfaces.Services;
using LeaveManagement.Core.Entities;
using LeaveManagement.Core.Enums;

namespace LeaveManagement.Application.Commands
{
    // 1. Command Class
    public class UpdateLeaveRequestCommand
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }
    }

    // 2. Command Handler Class
    public class UpdateLeaveRequestCommandHandler
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IValidator<UpdateLeaveRequestCommand> _validator;
        private readonly ILogger<UpdateLeaveRequestCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly ILeaveRequestService _leaveRequestService;


        public UpdateLeaveRequestCommandHandler(
            ILeaveRequestRepository leaveRequestRepository,
            IValidator<UpdateLeaveRequestCommand> validator,
            ILogger<UpdateLeaveRequestCommandHandler> logger,
            IMapper mapper,
            ILeaveRequestService leaveRequestService)
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _leaveRequestService = leaveRequestService ?? throw new ArgumentNullException(nameof(leaveRequestService));

        }

        public async Task HandleAsync(UpdateLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            // Input validation
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for UpdateLeaveRequestCommand: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            // Get the leave request to update
            var leaveRequestToUpdate = await _leaveRequestRepository.GetByIdAsync(command.Id);
            if (leaveRequestToUpdate == null)
            {
                _logger.LogWarning("LeaveRequest with ID {LeaveRequestId} not found.", command.Id);
                return;
            }

            // Map the command to the entity using AutoMapper
            _mapper.Map(command, leaveRequestToUpdate); // Maps from command to leaveRequestToUpdate


            // Implement Business Rule Validation
            await _leaveRequestService.ValidateLeaveRequest(leaveRequestToUpdate);

            // Update in the database
            await _leaveRequestRepository.UpdateAsync(leaveRequestToUpdate);
            _logger.LogInformation("LeaveRequest with ID {LeaveRequestId} updated.", leaveRequestToUpdate.Id);
        }
    }

    // 3. Command Validator
    public class UpdateLeaveRequestCommandValidator : AbstractValidator<UpdateLeaveRequestCommand>
    {
        public UpdateLeaveRequestCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.");
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
            RuleFor(c => c.Status)
                .IsInEnum().WithMessage("Status is invalid.");
        }
    }
}
