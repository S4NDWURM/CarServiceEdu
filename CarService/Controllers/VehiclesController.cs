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
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _service;
        public VehiclesController(IVehicleService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<VehicleResponse>>> Get()
        {
            var items = await _service.GetAllVehicles();
            var response = items.Select(i => new VehicleResponse(i.Id, i.VIN, i.Year, i.GenerationId));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist,Client")]
        public async Task<ActionResult<VehicleResponse>> GetById(Guid id)
        {
            var item = await _service.GetVehicleById(id);
            if (item == null)
            {
                return NotFound($"Vehicle with id {id} not found");
            }

            var response = new VehicleResponse(item.Id, item.VIN, item.Year, item.GenerationId);
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Specialist,Client")]
        public async Task<ActionResult<Guid>> Create([FromBody] VehicleRequest request)
        {
            var (model, error) = Vehicle.Create(Guid.NewGuid(), request.VIN, request.Year, request.GenerationId);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            var id = await _service.CreateVehicle(model);
            return Ok(id);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Specialist,Client")]
        public async Task<ActionResult<List<VehicleResponse>>> GetByVIN([FromQuery] string vin)
        {
            if (string.IsNullOrEmpty(vin))
                return BadRequest("VIN must be provided.");

            var items = await _service.GetVehiclesByVIN(vin);
            if (items == null || !items.Any())
                return NotFound($"No vehicles found with VIN {vin}");

            var response = items.Select(i => new VehicleResponse(i.Id, i.VIN, i.Year, i.GenerationId));
            return Ok(response);
        }


        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] VehicleRequest request)
        {
            var updated = await _service.UpdateVehicle(id, request.VIN, request.Year, request.GenerationId);
            return Ok(updated);
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteVehicle(id);
            return Ok(id);
        }
    }
}