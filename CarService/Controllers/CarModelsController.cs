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
    public class CarModelsController : ControllerBase
    {
        private readonly ICarModelService _service;
        public CarModelsController(ICarModelService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Client,Specialist,Admin")]
        public async Task<ActionResult<List<CarModelResponse>>> Get()
        {
            var items = await _service.GetAllCarModels();
            var response = items.Select(i => new CarModelResponse(i.Id, i.Name, i.CarBrandId));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Client,Specialist,Admin")]
        public async Task<ActionResult<CarModelResponse>> GetById(Guid id)
        {
            var item = await _service.GetCarModelById(id);
            if (item == null)
            {
                return NotFound($"CarModel with id {id} not found");
            }

            var response = new CarModelResponse(item.Id, item.Name, item.CarBrandId);
            return Ok(response);
        }

        [HttpGet("brand/{carBrandId:guid}")]
        [Authorize(Roles = "Client,Specialist,Admin")]
        public async Task<ActionResult<List<CarModelResponse>>> GetByCarBrandId(Guid carBrandId)
        {
            var items = await _service.GetCarModelsByCarBrandId(carBrandId);
            if (items == null || !items.Any())
            {
                return NotFound($"No CarModels found for CarBrandId {carBrandId}");
            }

            var response = items.Select(i => new CarModelResponse(i.Id, i.Name, i.CarBrandId));
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] CarModelRequest request)
        {
            var (model, error) = CarModel.Create(Guid.NewGuid(), request.Name, request.CarBrandId);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            var id = await _service.CreateCarModel(model);
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] CarModelRequest request)
        {
            var updated = await _service.UpdateCarModel(id, request.Name, request.CarBrandId);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteCarModel(id);
            return Ok(id);
        }
    }
}