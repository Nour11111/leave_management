using LeaveManagement.Application.Interfaces.Repositories;
using LeaveManagement.Core.Entities;

namespace LeaveManagement.Application.Queries
{
    // 1. Query Class: GetEmployeeByIdQuery (Get by ID)
    public class GetEmployeeByIdQuery
    {
        public int Id { get; set; }
    }

    // 2. Query Handler Class: GetEmployeeByIdHandler
    public class GetEmployeeByIdHandler
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<GetEmployeeByIdHandler> _logger;

        public GetEmployeeByIdHandler(IEmployeeRepository employeeRepository, ILogger<GetEmployeeByIdHandler> logger)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Employee> HandleAsync(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting employee with ID: {EmployeeId}", query.Id);
            var employee = await _employeeRepository.GetByIdAsync(query.Id);
            if (employee == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found.", query.Id);
                return null; // Or throw an exception, or return a default value
            }
            return employee;
        }
    }
}