using LeaveManagement.Application.Commands;
using LeaveManagement.Application.Queries;
using LeaveManagement.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Controllers
{
        [ApiController]
        [Route("api/employees")]
        public class EmployeeController : ControllerBase
        {
            private readonly IMediator _mediator;
            private readonly ILogger<EmployeeController> _logger;

            public EmployeeController(IMediator mediator, ILogger<EmployeeController> logger)
            {
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Employee>> GetEmployeeById(int id)
            {
                var query = new GetEmployeeByIdQuery { Id = id };
                var employee = await _mediator.Send(query);

                if (employee == null)
                {
                    return NotFound(); // Returns 404 Not Found
                }

                return Ok(employee); // Returns 200 OK with the employee data
            }

            [HttpGet]
            public async Task<ActionResult<List<Employee>>> GetAllEmployees()
            {
                var query = new GetAllEmployeesQuery();
                var employees = await _mediator.Send(query);
                return Ok(employees); // Returns 200 OK with the list of employees
            }

            [HttpPost]
            public async Task<ActionResult<int>> CreateEmployee([FromBody] CreateEmployeeCommand command)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Returns 400 Bad Request with validation errors
                }

                var employeeId = await _mediator.Send(command);
                _logger.LogInformation("Employee created with ID: {EmployeeId}", employeeId);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeId }, employeeId); // Returns 201 Created
            }

            [HttpPut("{id}")]
            public async Task<ActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeCommand command)
            {
                if (id != command.Id)
                {
                    return BadRequest("ID in route and body do not match."); // Returns 400 Bad Request
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Returns 400 Bad Request
                }

                try
                {
                    await _mediator.Send(command);
                    return NoContent(); // Returns 204 No Content (successful update)
                }
                catch (Exception ex) //catch the ValidationException
                {
                    _logger.LogError(ex, "Error updating employee with ID {EmployeeId}", id);
                    return BadRequest(ex.Message);
                }

            }
        }
    }
