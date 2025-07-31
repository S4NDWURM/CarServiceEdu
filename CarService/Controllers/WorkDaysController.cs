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
    public class WorkDaysController : ControllerBase
    {
        private readonly IWorkDayService _service;
        public WorkDaysController(IWorkDayService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<WorkDayResponse>>> Get()
        {
            var items = await _service.GetAllWorkDays();
            var response = items.Select(i => new WorkDayResponse(i.Id, i.EmployeeId, i.TypeOfDayId, i.StartTime, i.EndTime));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<WorkDayResponse>> GetById(Guid id)
        {
            var item = await _service.GetWorkDayById(id);
            var response = new WorkDayResponse(item.Id, item.EmployeeId, item.TypeOfDayId, item.StartTime, item.EndTime);
            return Ok(response);
        }

        [HttpGet("employee/{employeeId:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<WorkDayResponse>>> GetByEmployeeId(Guid employeeId)
        {
            var items = await _service.GetWorkDaysByEmployeeId(employeeId);
            var response = items.Select(i => new WorkDayResponse(i.Id, i.EmployeeId, i.TypeOfDayId, i.StartTime, i.EndTime));
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] WorkDayRequest request)
        {
            var (model, error) = WorkDay.Create(Guid.NewGuid(), request.EmployeeId, request.TypeOfDayId, request.StartTime, request.EndTime);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            var id = await _service.CreateWorkDay(model);
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] WorkDayRequest request)
        {
            var updated = await _service.UpdateWorkDay(id, request.EmployeeId, request.TypeOfDayId, request.StartTime, request.EndTime);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteWorkDay(id);
            return Ok(id);
        }
    }
}