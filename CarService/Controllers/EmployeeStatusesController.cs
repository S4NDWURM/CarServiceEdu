using CarService.API.Contracts;
using CarService.Application.Services;
using CarService.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeStatusesController : ControllerBase
    {
        private readonly IEmployeeStatusService _service;
        public EmployeeStatusesController(IEmployeeStatusService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<EmployeeStatusResponse>>> Get()
        {
            var items = await _service.GetAllEmployeeStatuses();
            var response = items.Select(i => new EmployeeStatusResponse(i.Id, i.Name));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<EmployeeStatusResponse>> GetById(Guid id)
        {
            var item = await _service.GetEmployeeStatusById(id);
            if (item == null)
            {
                return NotFound($"EmployeeStatus with id {id} not found");
            }

            var response = new EmployeeStatusResponse(item.Id, item.Name);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] EmployeeStatusRequest request)
        {
            var (model, error) = EmployeeStatus.Create(Guid.NewGuid(), request.Name);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            var id = await _service.CreateEmployeeStatus(model);
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] EmployeeStatusRequest request)
        {
            var updated = await _service.UpdateEmployeeStatus(id, request.Name);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteEmployeeStatus(id);
            return Ok(id);
        }
    }
}
