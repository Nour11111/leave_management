using LeaveManagement.Application.Interfaces.Repositories;
using LeaveManagement.Application.Specifications;
using LeaveManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace LeaveManagement.Infrastructure.Repositories
{
    
        public class LeaveRequestRepository : ILeaveRequestRepository
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly ILogger<LeaveRequestRepository> _logger;

            public LeaveRequestRepository(ApplicationDbContext dbContext, ILogger<LeaveRequestRepository> logger)
            {
                _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }
            public async Task<List<LeaveRequest>> GetAllAsync()
            {
                _logger.LogInformation("Getting all LeaveRequests");
                return await _dbContext.LeaveRequests.ToListAsync(); 
            }
            public async Task<LeaveRequest> GetByIdAsync(int id)
                {
                    _logger.LogInformation("Getting LeaveRequest by ID: {Id}", id);
                    return await _dbContext.LeaveRequests.FindAsync(id);
                }

                public async Task<List<LeaveRequest>> GetByEmployeeIdAsync(int employeeId)
                {
                    _logger.LogInformation("Getting LeaveRequests by Employee ID: {EmployeeId}", employeeId);
                    return await _dbContext.LeaveRequests
                        .Where(lr => lr.EmployeeId == employeeId)
                        .ToListAsync();
                }

                public async Task AddAsync(LeaveRequest leaveRequest)
                {
                    _logger.LogInformation("Adding new LeaveRequest");
                    _dbContext.LeaveRequests.Add(leaveRequest);
                    await _dbContext.SaveChangesAsync();
                }

                public async Task UpdateAsync(LeaveRequest leaveRequest)
                {
                    _logger.LogInformation("Updating LeaveRequest with ID: {Id}", leaveRequest.Id);
                    _dbContext.Entry(leaveRequest).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }

                public async Task DeleteAsync(LeaveRequest leaveRequest)
                {
                    _logger.LogInformation("Deleting LeaveRequest with ID: {Id}", leaveRequest.Id);
                    _dbContext.LeaveRequests.Remove(leaveRequest);
                    await _dbContext.SaveChangesAsync();
                }

            public async Task<List<LeaveRequest>> GetAllAsync(LeaveRequestSpecification specification)
            {
                _logger.LogInformation("Getting all LeaveRequests with specification");

                IQueryable<LeaveRequest> query = _dbContext.LeaveRequests;

                // Apply filtering using Dynamic LINQ
                if (specification != null)
                {
                    var filters = new List<string>();
                    var values = new List<object>();
                    int parameterIndex = 0;

                    if (specification.EmployeeId.HasValue)
                    {
                        filters.Add($"EmployeeId == @{parameterIndex}");
                        values.Add(specification.EmployeeId.Value);
                        parameterIndex++;
                    }
                    if (specification.LeaveType.HasValue)
                    {
                        filters.Add($"LeaveType == @{parameterIndex}");
                        values.Add(specification.LeaveType.Value);
                        parameterIndex++;
                    }
                    if (specification.Status.HasValue)
                    {
                        filters.Add($"Status == @{parameterIndex}");
                        values.Add(specification.Status.Value);
                        parameterIndex++;
                    }
                    if (specification.StartDate.HasValue)
                    {
                        filters.Add($"StartDate >= @{parameterIndex}");
                        values.Add(specification.StartDate.Value);
                        parameterIndex++;
                    }
                    if (specification.EndDate.HasValue)
                    {
                        filters.Add($"EndDate <= @{parameterIndex}");
                        values.Add(specification.EndDate.Value);
                        parameterIndex++;
                    }
                    if (!string.IsNullOrEmpty(specification.ReasonKeyword))
                    {
                        filters.Add($"Reason.Contains(@{parameterIndex})");
                        values.Add(specification.ReasonKeyword);
                        parameterIndex++;
                    }

                    if (filters.Any())
                    {
                        query = query.Where(string.Join(" and ", filters), values.ToArray());
                    }

                    // Apply sorting using Dynamic LINQ
                    if (!string.IsNullOrEmpty(specification.SortBy))
                    {
                        query = query.OrderBy($"{specification.SortBy} {specification.SortOrder.ToString().ToLower()}");
                    }

                    // Apply pagination
                    if (specification.Page.HasValue && specification.PageSize.HasValue)
                    {
                        query = query.Skip((specification.Page.Value - 1) * specification.PageSize.Value)
                                     .Take(specification.PageSize.Value);
                    }
                }

                return await query.ToListAsync();
        
                  }

        }
            }
