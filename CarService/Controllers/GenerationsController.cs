using CarService.API.Contracts;
using CarService.Application.Services;
using CarService.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerationsController : ControllerBase
    {
        private readonly IGenerationService _service;
        public GenerationsController(IGenerationService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist,Client")]
        public async Task<ActionResult<List<GenerationResponse>>> Get()
        {
            var items = await _service.GetAllGenerations();
            var response = items.Select(g => new GenerationResponse(
                g.Id, g.CarModelId, g.Name, g.StartYear, g.EndYear));

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist,Client")]
        public async Task<ActionResult<GenerationResponse>> GetById(Guid id)
        {
            var item = await _service.GetGenerationById(id);
            if (item == null)
            {
                return NotFound($"Generation with id {id} not found");
            }

            var response = new GenerationResponse(item.Id, item.CarModelId, item.Name, item.StartYear, item.EndYear);
            return Ok(response);
        }

        [HttpGet("carModel/{carModelId:guid}")]
        [Authorize(Roles = "Admin,Specialist,Client")]
        public async Task<ActionResult<List<GenerationResponse>>> GetByCarModelId(Guid carModelId)
        {
            var items = await _service.GetGenerationsByCarModelId(carModelId);
            if (items == null || !items.Any())
            {
                return NotFound($"No Generations found for CarModelId {carModelId}");
            }

            var response = items.Select(g => new GenerationResponse(
                g.Id, g.CarModelId, g.Name, g.StartYear, g.EndYear));

            return Ok(response);
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] GenerationRequest request)
        {
            var (model, error) = Generation.Create(Guid.NewGuid(),
                                                    request.CarModelId,
                                                   request.Name,
                                                   request.StartYear,
                                                   request.EndYear);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);

            var id = await _service.CreateGeneration(model);
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id,
                [FromBody] GenerationRequest request)
        {
            var updated = await _service.UpdateGeneration(id, request.CarModelId,
                request.Name, request.StartYear, request.EndYear);

            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteGeneration(id);
            return Ok(id);
        }
    }
}
