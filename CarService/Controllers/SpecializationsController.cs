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
    public class SpecializationsController : ControllerBase
    {
        private readonly ISpecializationService _service;
        public SpecializationsController(ISpecializationService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin, Specialist")]
        public async Task<ActionResult<List<SpecializationResponse>>> Get()
        {
            var items = await _service.GetAllSpecializations();
            var response = items.Select(i => new SpecializationResponse(i.Id, i.Name));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Specialist")]
        public async Task<ActionResult<SpecializationResponse>> GetById(Guid id)
        {
            var item = await _service.GetSpecializationById(id);
            var response = new SpecializationResponse(item.Id, item.Name);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] SpecializationRequest request)
        {
            var (model, error) = Specialization.Create(Guid.NewGuid(), request.Name);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            var id = await _service.CreateSpecialization(model);
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] SpecializationRequest request)
        {
            var updated = await _service.UpdateSpecialization(id, request.Name);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteSpecialization(id);
            return Ok(id);
        }
    }
}