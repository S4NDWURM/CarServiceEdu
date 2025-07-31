using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarService.API.Contracts;
using CarService.Application.Services;
using CarService.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace CarService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlannedWorkEmployeesController : ControllerBase
    {
        private readonly IPlannedWorkEmployeeService _service;
        public PlannedWorkEmployeesController(IPlannedWorkEmployeeService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<PlannedWorkEmployeeResponse>>> Get()
        {
            var items = await _service.GetAllPlannedWorkEmployees();
            var response = items.Select(i => new PlannedWorkEmployeeResponse(i.PlannedWorkId, i.EmployeeId));
            return Ok(response);
        }

        [HttpGet("{plannedWorkId:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<PlannedWorkEmployeeResponse>>> GetById(Guid plannedWorkId)
        {
            var items = await _service.GetPlannedWorkEmployeesById(plannedWorkId);
            var response = items.Select(i => new PlannedWorkEmployeeResponse(i.PlannedWorkId, i.EmployeeId));
            return Ok(response);
        }

        [HttpGet("details/{plannedWorkId:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<PlannedWorkEmployeeWithDetailsResponse>>> GetByIdDetailed(Guid plannedWorkId)
        {
            var items = await _service.GetPlannedWorkEmployeesWithDetailsById(plannedWorkId);
            var response = items.Select(i => new PlannedWorkEmployeeWithDetailsResponse(i.PlannedWorkId, i.EmployeeId, i.LastName, i.FirstName, i.MiddleName));
            return Ok(response);
        }



        [HttpPost]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<IActionResult> Create([FromBody] PlannedWorkEmployeeRequest request)
        {
            var model = PlannedWorkEmployee.Create(request.PlannedWorkId, request.EmployeeId).Item;
            await _service.CreatePlannedWorkEmployee(model);
            return Ok();
        }

        [HttpDelete("{plannedWorkId:guid}/{employeeId:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<IActionResult> Delete(Guid plannedWorkId, Guid employeeId)
        {
            await _service.DeletePlannedWorkEmployee(plannedWorkId, employeeId);
            return Ok();
        }
    }
}