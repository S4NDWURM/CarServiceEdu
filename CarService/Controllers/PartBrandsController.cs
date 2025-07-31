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
    public class PartBrandsController : ControllerBase
    {
        private readonly IPartBrandService _service;
        public PartBrandsController(IPartBrandService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<PartBrandResponse>>> Get()
        {
            var items = await _service.GetAllPartBrands();
            var response = items.Select(i => new PartBrandResponse(i.Id, i.Name));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<PartBrandResponse>> GetById(Guid id)
        {
            var item = await _service.GetPartBrandById(id);
            if (item == null)
            {
                return NotFound($"PartBrand with id {id} not found");
            }

            var response = new PartBrandResponse(item.Id, item.Name);
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] PartBrandRequest request)
        {
            var (model, error) = PartBrand.Create(Guid.NewGuid(), request.Name);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            var id = await _service.CreatePartBrand(model);
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] PartBrandRequest request)
        {
            var updated = await _service.UpdatePartBrand(id, request.Name);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeletePartBrand(id);
            return Ok(id);
        }
    }
}