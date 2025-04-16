using LeaveManagement.Application.Interfaces.Repositories;
using LeaveManagement.Application.Specifications;
using LeaveManagement.Core.Entities;
using LeaveManagement.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext; // Inject the DbContext
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(ApplicationDbContext dbContext, ILogger<EmployeeRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            _logger.LogInformation("Getting employee by ID: {Id}", id);
            return await _dbContext.Employees.FindAsync(id);
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            _logger.LogInformation("Getting all employees");
            return await _dbContext.Employees.ToListAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            _logger.LogInformation("Adding new employee: {FullName}", employee.FullName);
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync(); // Ensure changes are persisted
        }

        public async Task UpdateAsync(Employee employee)
        {
            _logger.LogInformation("Updating employee with ID: {Id}", employee.Id);
            _dbContext.Entry(employee).State = EntityState.Modified; // Explicitly mark as modified
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _logger.LogInformation("Deleting employee with ID: {Id}", employee.Id);
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }
    }
}
