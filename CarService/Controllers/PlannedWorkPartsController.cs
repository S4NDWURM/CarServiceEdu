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
    public class PlannedWorkPartsController : ControllerBase
    {
        private readonly IPlannedWorkPartService _service;
        public PlannedWorkPartsController(IPlannedWorkPartService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<PlannedWorkPartResponse>>> Get()
        {
            var items = await _service.GetAllPlannedWorkParts();
            var response = items.Select(i => new PlannedWorkPartResponse(i.PlannedWorkId, i.PartId, i.Quantity));
            return Ok(response);
        }

        [HttpGet("{plannedWorkId:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<PlannedWorkPartResponse>>> GetById(Guid plannedWorkId)
        {
            var items = await _service.GetPlannedWorkPartsById(plannedWorkId);
            var response = items.Select(i => new PlannedWorkPartResponse(i.PlannedWorkId, i.PartId, i.Quantity));
            return Ok(response);
        }

        [HttpGet("details/{plannedWorkId:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<PlannedWorkPartWithDetailsResponse>>> GetByIdDetailed(Guid plannedWorkId)
        {
            var items = await _service.GetPlannedWorkPartsWithDetails(plannedWorkId);
            var response = items.Select(i => new PlannedWorkPartWithDetailsResponse(i.PartId, i.Name, i.Article, i.Cost, i.Quantity, i.BrandName));
            return Ok(response);
        }



        [HttpPost]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<IActionResult> Create([FromBody] PlannedWorkPartRequest request)
        {
            var model = PlannedWorkPart.Create(request.PlannedWorkId, request.PartId, request.Quantity).Item;
            await _service.CreatePlannedWorkPart(model);
            return Ok();
        }

        [HttpDelete("{plannedWorkId:guid}/{partId:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<IActionResult> Delete(Guid plannedWorkId, Guid partId)
        {
            await _service.DeletePlannedWorkPart(plannedWorkId, partId);
            return Ok();
        }
    }
}