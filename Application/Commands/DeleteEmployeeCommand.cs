using LeaveManagement.Application.Interfaces.Repositories;

namespace LeaveManagement.Application.Commands
{
    // 1. Command Class
    public class DeleteEmployeeCommand
    {
        public int Id { get; set; } // Include the ID of the employee to delete
    }

    // 2. Command Handler Class
    public class DeleteEmployeeHandler
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<DeleteEmployeeHandler> _logger;

        public DeleteEmployeeHandler(
            IEmployeeRepository employeeRepository,
            ILogger<DeleteEmployeeHandler> logger)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            // Get the employee to delete
            var employeeToDelete = await _employeeRepository.GetByIdAsync(command.Id);
            if (employeeToDelete == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found.", command.Id);
                return; // Or throw an exception, or return a value indicating failure
            }

            // Delete the employee from the database
            await _employeeRepository.DeleteAsync(employeeToDelete);
            _logger.LogInformation("Employee with ID {EmployeeId} deleted.", command.Id);
        }
    } 
}
