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
    public class EmployeeSpecializationsController : ControllerBase
    {
        private readonly IEmployeeSpecializationService _service;
        public EmployeeSpecializationsController(IEmployeeSpecializationService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<EmployeeSpecializationResponse>>> Get()
        {
            var items = await _service.GetAllEmployeeSpecializations();
            var response = items.Select(i => new EmployeeSpecializationResponse(i.EmployeeId, i.SpecializationId));
            return Ok(response);
        }

        [HttpGet("{employeeId:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<EmployeeSpecializationResponse>> GetById(Guid employeeId)
        {
            var item = await _service.GetEmployeeSpecializationById(employeeId);
            var response = new EmployeeSpecializationResponse(item.EmployeeId, item.SpecializationId);
            return Ok(response);
        }

        [HttpGet("detailed/{employeeId:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<EmployeeSpecializationWithDetailsResponse>>> GetByIdDetailed(Guid employeeId)
        {
            var items = await _service.GetEmployeeSpecializationByIdDetailed(employeeId);
            var response = items.Select(i => new EmployeeSpecializationWithDetailsResponse(i.EmployeeId, i.SpecializationId, i.Name, i.Description));
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] EmployeeSpecializationRequest request)
        {
            var model = EmployeeSpecialization.Create(request.EmployeeId, request.SpecializationId).Item;
            await _service.CreateEmployeeSpecialization(model);
            return Ok();
        }

        [HttpDelete("{employeeId:guid}/{specializationId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid employeeId, Guid specializationId)
        {
            await _service.DeleteEmployeeSpecialization(employeeId, specializationId);
            return Ok();
        }
    }
}