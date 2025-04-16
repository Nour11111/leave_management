using AutoMapper;
using FluentValidation;
using LeaveManagement.Application.Interfaces.Repositories;

namespace LeaveManagement.Application.Commands
{
    public class UpdateEmployeeCommand
    {
        public int Id { get; set; } // Include the ID for the employee to update
        public string FullName { get; set; }
        public string Department { get; set; }
        public DateTime JoiningDate { get; set; }
    }

    // 2. Command Handler Class
    public class UpdateEmployeeHandler
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IValidator<UpdateEmployeeCommand> _validator;
        private readonly ILogger<UpdateEmployeeHandler> _logger;
        private readonly IMapper _mapper;


        public UpdateEmployeeHandler(
            IEmployeeRepository employeeRepository,
            IValidator<UpdateEmployeeCommand> validator,
            ILogger<UpdateEmployeeHandler> logger,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task HandleAsync(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            // Input validation
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for UpdateEmployeeCommand: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            // Get the employee to update
            var employeeToUpdate = await _employeeRepository.GetByIdAsync(command.Id);
            if (employeeToUpdate == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found.", command.Id);
                throw new ValidationException(validationResult.Errors);
            }

            // Map the command to the entity

            _mapper.Map(command, employeeToUpdate);


            // Update in the database
            await _employeeRepository.UpdateAsync(employeeToUpdate);
            _logger.LogInformation("Employee with ID {EmployeeId} updated.", employeeToUpdate.Id);
        }
    }

    // 3. Command Validator
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required."); // Ensure ID is provided for updates

            RuleFor(c => c.FullName)
                .NotEmpty().WithMessage("FullName is required.")
                .MaximumLength(100).WithMessage("FullName must not exceed 100 characters.");

            RuleFor(c => c.Department)
                .NotEmpty().WithMessage("Department is required.")
                .MaximumLength(50).WithMessage("Department must not exceed 50 characters.");

            RuleFor(c => c.JoiningDate)
                .NotEmpty().WithMessage("JoiningDate is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("JoiningDate must be in the past or present.");
        }
    }
}
