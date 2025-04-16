using LeaveManagement.Application.Interfaces.Repositories;
using LeaveManagement.Core.Entities;

namespace LeaveManagement.Application.Queries
{
    public class GetAllEmployeesQuery
    {
        // No parameters for "get all"
    }

    // 4. Query Handler Class: GetEmployeesHandler
    public class GetEmployeesHandler
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<GetEmployeesHandler> _logger;

        public GetEmployeesHandler(IEmployeeRepository employeeRepository, ILogger<GetEmployeesHandler> logger)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Employee>> HandleAsync(GetAllEmployeesQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all employees");
            var employees = await _employeeRepository.GetAllAsync();
            return employees;
        }
    }
}