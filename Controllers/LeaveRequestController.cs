using LeaveManagement.Application.Commands;
using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.Queries;
using LeaveManagement.Application.Specifications;
using LeaveManagement.Core.Entities;
using LeaveManagement.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Controllers
{
    [ApiController]
    [Route("api/leaverequests")]
    public class LeaveRequestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LeaveRequestController> _logger;

        public LeaveRequestController(IMediator mediator, ILogger<LeaveRequestController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaveRequest>>> GetAllLeaveRequests()
        {
            var query = new GetAllLeaveRequestsQuery();
            var leaveRequests = await _mediator.Send(query);
            return Ok(leaveRequests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequest>> GetLeaveRequestById(int id)
        {
            var query = new GetLeaveRequestByIdQuery { Id = id };
            var leaveRequest = await _mediator.Send(query);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            return Ok(leaveRequest);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<List<LeaveRequestByEmployeeIdDto>>> GetLeaveRequestsByEmployeeId(int employeeId)
        {
            var query = new GetLeaveRequestsByEmployeeIdQuery { EmployeeId = employeeId };
            var leaveRequests = await _mediator.Send(query);
            return Ok(leaveRequests);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateLeaveRequest([FromBody] CreateLeaveRequestCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var leaveRequestId = await _mediator.Send(command);
            _logger.LogInformation("Leave request created with ID: {LeaveRequestId}", leaveRequestId);
            return CreatedAtAction(nameof(GetLeaveRequestById), new { id = leaveRequestId }, leaveRequestId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLeaveRequest(int id, [FromBody] UpdateLeaveRequestCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in route and body do not match.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating leave request with ID {LeaveRequestId}", id);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("filter")]
        public async Task<ActionResult<List<LeaveRequest>>> GetLeaveRequestsWithFilter(
            int? employeeId,
            LeaveType? leaveType,
            LeaveStatus? status,
            DateTime? startDate,
            DateTime? endDate,
            int? page,
            int? pageSize,
            string sortBy,
            SortOrder? sortOrder,
            string reasonKeyword)
        {
            //  Create a new LeaveRequestSpecification .
            var specification = new LeaveRequestSpecification
            {
                EmployeeId = employeeId,
                LeaveType = leaveType,
                Status = status,
                StartDate = startDate,
                EndDate = endDate,
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder ?? SortOrder.Ascending, 
                ReasonKeyword = reasonKeyword
            };

            var query = new GetLeaveRequestsWithFilterQuery { Specification = specification }; // Pass the specification
            var leaveRequests = await _mediator.Send(query);
            return Ok(leaveRequests);
        }
    } 
    }