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
    public class WorksController : ControllerBase
    {
        private readonly IWorkService _service;
        public WorksController(IWorkService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<WorkResponse>>> Get()
        {
            var items = await _service.GetAllWorks();
            var response = items.Select(i => new WorkResponse(i.Id, i.Name, i.Description, i.Cost));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<WorkResponse>> GetById(Guid id)
        {
            var item = await _service.GetWorkById(id);
            if (item == null)
            {
                return NotFound($"Work with id {id} not found");
            }

            var response = new WorkResponse(item.Id, item.Name, item.Description, item.Cost);
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] WorkRequest request)
        {
            var (model, error) = Work.Create(Guid.NewGuid(), request.Name, request.Description, request.Cost);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            var id = await _service.CreateWork(model);
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] WorkRequest request)
        {
            var updated = await _service.UpdateWork(id, request.Name, request.Description, request.Cost);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteWork(id);
            return Ok(id);
        }
    }
}