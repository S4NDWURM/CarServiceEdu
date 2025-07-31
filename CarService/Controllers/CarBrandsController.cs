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
    public class CarBrandsController : ControllerBase
    {
        private readonly ICarBrandService _service;
        public CarBrandsController(ICarBrandService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Client,Specialist,Admin")]
        public async Task<ActionResult<List<CarBrandResponse>>> Get()
        {
            var items = await _service.GetAllCarBrands();
            var response = items.Select(i => new CarBrandResponse(i.Id, i.Name));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Client,Specialist,Admin")]
        public async Task<ActionResult<CarBrandResponse>> GetById(Guid id)
        {
            var item = await _service.GetCarBrandById(id);
            if (item == null)
            {
                return NotFound($"CarBrand with id {id} not found");
            }

            var response = new CarBrandResponse(item.Id, item.Name);
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] CarBrandRequest request)
        {
            var (model, error) = CarBrand.Create(Guid.NewGuid(), request.Name);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            var id = await _service.CreateCarBrand(model);
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] CarBrandRequest request)
        {
            var updated = await _service.UpdateCarBrand(id, request.Name);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteCarBrand(id);
            return Ok(id);
        }
    }
}