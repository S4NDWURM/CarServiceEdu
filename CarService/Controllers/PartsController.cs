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
    public class PartsController : ControllerBase
    {
        private readonly IPartService _service;
        public PartsController(IPartService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<PartResponse>>> Get()
        {
            var items = await _service.GetAllParts();
            var response = items.Select(i => new PartResponse(i.Id, i.Name, i.Article, i.Cost, i.PartBrandId));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<PartResponse>> GetById(Guid id)
        {
            var item = await _service.GetPartById(id);
            if (item == null)
            {
                return NotFound($"Part with id {id} not found");
            }

            var response = new PartResponse(item.Id, item.Name, item.Article, item.Cost, item.PartBrandId);
            return Ok(response);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<PartResponse>>> SearchByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name query parameter is required.");
            }

            var items = await _service.SearchPartsByName(name);
            if (items == null || !items.Any())
            {
                return NotFound($"No parts found matching '{name}'.");
            }

            var response = items.Select(i => new PartResponse(i.Id, i.Name, i.Article, i.Cost, i.PartBrandId));
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] PartRequest request)
        {
            var (model, error) = Part.Create(Guid.NewGuid(), request.Name, request.Article, request.Cost, request.PartBrandId);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            var id = await _service.CreatePart(model);
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] PartRequest request)
        {
            var updated = await _service.UpdatePart(id, request.Name, request.Article, request.Cost, request.PartBrandId);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeletePart(id);
            return Ok(id);
        }
    }
}