using LeaveManagement.Application.Commands;
using LeaveManagement.Core.Entities;
using FluentValidation;
using LeaveManagement.Application.Interfaces.Repositories;
using AutoMapper;


namespace LeaveManagement.Application.Commands
{
    // 1. Command Class
    public class CreateEmployeeCommand
    {
        public string FullName { get; set; }
        public string Department { get; set; }
        public DateTime JoiningDate { get; set; }
    }

    // 2. Command Handler Class
    public class CreateEmployeeHandler
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IValidator<CreateEmployeeCommand> _validator;
        private readonly ILogger<CreateEmployeeHandler> _logger;
        private readonly IMapper _mapper;


        public CreateEmployeeHandler(
            IEmployeeRepository employeeRepository,
            IValidator<CreateEmployeeCommand> validator,
            ILogger<CreateEmployeeHandler> logger,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> HandleAsync(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            // Input validation
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateEmployeeCommand: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors); // Throw ValidationException
            }

            // Map the command to the entity
            var employee = _mapper.Map<Employee>(command);
            
            // Add to the database using the repository
            await _employeeRepository.AddAsync(employee);

            // Return the new employee's ID
            return employee.Id;
        }
    }

    // 3. Command Validator
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
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

