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
    public class PlannedWorksController : ControllerBase
    {
        private readonly IPlannedWorkService _service;
        public PlannedWorksController(IPlannedWorkService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<PlannedWorkResponse>>> Get()
        {
            var items = await _service.GetAllPlannedWorks();
            var response = items.Select(i => new PlannedWorkResponse(i.Id, i.PlanDate, i.ExpectedEndDate, i.TotalCost, i.WorkId, i.RequestId, i.StatusId));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<PlannedWorkResponse>> GetById(Guid id)
        {
            var item = await _service.GetPlannedWorkById(id);
            if (item == null)
            {
                return NotFound($"PlannedWork with id {id} not found");
            }

            var response = new PlannedWorkResponse(item.Id, item.PlanDate, item.ExpectedEndDate, item.TotalCost, item.WorkId, item.RequestId, item.StatusId);
            return Ok(response);
        }

        [HttpGet("details/{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<PlannedWorkWithDetailsResponse>> GetByIdDetailed(Guid id)
        {
            var item = await _service.GetPlannedWorkWithDetailsById(id);
            if (item == null)
            {
                return NotFound($"PlannedWork with id {id} not found");
            }

            var response = new PlannedWorkWithDetailsResponse(
                item.Id,
                item.PlanDate,
                item.ExpectedEndDate,
                item.TotalCost,
                item.WorkId,
                item.RequestId,
                item.StatusId,
                item.WorkName,
                item.WorkDesctiption,
                item.WorkCost,
                item.StatusName
            );
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<Guid>> Create([FromBody] PlannedWorkRequest request)
        {
            var (model, error) = PlannedWork.Create(Guid.NewGuid(), request.PlanDate, request.ExpectedEndDate, request.TotalCost, request.WorkId, request.RequestId, request.StatusId);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            var id = await _service.CreatePlannedWork(model);
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] PlannedWorkRequest request)
        {
            var updated = await _service.UpdatePlannedWork(id, request.PlanDate, request.ExpectedEndDate, request.TotalCost, request.WorkId, request.RequestId, request.StatusId);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeletePlannedWork(id);
            return Ok(id);
        }
    }
}