using CarService.API.Contracts;
using CarService.Application.Services;
using CarService.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly IUserService _userService;
        public EmployeesController(IEmployeeService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpGet("me")]
        [Authorize(Roles = "Specialist")]
        public async Task<ActionResult<EmployeeResponse>> GetMe()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (Guid.TryParse(userId, out var userGuid))
            {
                var employeeId = await _userService.GetEmployeeIdByUserId(userGuid);
                var item = await _service.GetEmployeeById(employeeId);
                if (item == null)
                {
                    return NotFound($"Employee with id {employeeId} not found");
                }

                var response = new EmployeeResponse(
                    item.Id, item.LastName, item.FirstName, item.MiddleName, item.WorkExperience, item.HireDate, item.EmployeeStatus);
                return Ok(response);
            }

            return Unauthorized("Invalid user ID.");
        }

        [HttpPut("me")]
        [Authorize(Roles = "Specialist")]
        public async Task<ActionResult<Guid>> UpdateMe([FromBody] EmployeeRequest request)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (Guid.TryParse(userId, out var userGuid))
            {
                var employeeId = await _userService.GetEmployeeIdByUserId(userGuid);
                var updated = await _service.UpdateEmployee(employeeId, request.LastName, request.FirstName, request.MiddleName,
                    request.WorkExperience, request.HireDate, request.EmployeeStatus);
                return Ok(updated);
            }

            return Unauthorized("Invalid user ID.");
        }

        [HttpDelete("me")]
        [Authorize(Roles = "Specialist")]
        public async Task<ActionResult<Guid>> DeleteMe()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (Guid.TryParse(userId, out var userGuid))
            {
                var employeeId = await _userService.GetEmployeeIdByUserId(userGuid);
                await _service.DeleteEmployee(employeeId);
                return Ok(employeeId);
            }

            return Unauthorized("Invalid user ID.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<EmployeeResponse>>> Get()
        {
            var items = await _service.GetAllEmployees();
            var response = items.Select(i => new EmployeeResponse(i.Id, i.LastName, i.FirstName, i.MiddleName, i.WorkExperience, i.HireDate, i.EmployeeStatus));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<EmployeeResponse>> GetById(Guid id)
        {
            var item = await _service.GetEmployeeById(id);
            var response = new EmployeeResponse(item.Id, item.LastName, item.FirstName, item.MiddleName, item.WorkExperience, item.HireDate, item.EmployeeStatus);
            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] EmployeeRequest request)
        {
            var updated = await _service.UpdateEmployee(id, request.LastName, request.FirstName, request.MiddleName, request.WorkExperience, request.HireDate, request.EmployeeStatus);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteEmployee(id);
            return Ok(id);
        }
    }
}
