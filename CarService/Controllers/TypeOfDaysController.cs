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
    public class TypeOfDaysController : ControllerBase
    {
        private readonly ITypeOfDayService _service;
        public TypeOfDaysController(ITypeOfDayService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin, Specialist")]
        public async Task<ActionResult<List<TypeOfDayResponse>>> Get()
        {
            var items = await _service.GetAllTypeOfDays();
            var response = items.Select(i => new TypeOfDayResponse(i.Id, i.Name));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Specialist")]
        public async Task<ActionResult<TypeOfDayResponse>> GetById(Guid id)
        {
            var item = await _service.GetTypeOfDayById(id);
            var response = new TypeOfDayResponse(item.Id, item.Name);
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] TypeOfDayRequest request)
        {
            var (model, error) = TypeOfDay.Create(Guid.NewGuid(), request.Name);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);
            var id = await _service.CreateTypeOfDay(model);
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] TypeOfDayRequest request)
        {
            var updated = await _service.UpdateTypeOfDay(id, request.Name);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteTypeOfDay(id);
            return Ok(id);
        }
    }
}