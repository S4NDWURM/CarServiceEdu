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
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _service;
        public StatusController(IStatusService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin, Specialist,Client")]
        public async Task<ActionResult<List<StatusResponse>>> Get()
        {
            var items = await _service.GetAllStatuss();
            var response = items.Select(i => new StatusResponse(i.Id, i.Name));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Specialist, Client")]
        public async Task<ActionResult<StatusResponse>> GetById(Guid id)
        {
            var item = await _service.GetStatusById(id);
            if (item == null)
            {
                return NotFound($"Status with id {id} not found");
            }

            var response = new StatusResponse(item.Id, item.Name);
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] StatusRequest request)
        {
            var (model, error) = Status.Create(Guid.NewGuid(), request.Name);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            var id = await _service.CreateStatus(model);
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] StatusRequest request)
        {
            var updated = await _service.UpdateStatus(id, request.Name);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteStatus(id);
            return Ok(id);
        }
    }
}