using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarService.API.Contracts;
using CarService.Application.Services;
using CarService.Core.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace CarService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosticssController : ControllerBase
    {
        private readonly IDiagnosticsService _service;
        private readonly IUserService _userService;
        public DiagnosticssController(IDiagnosticsService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<DiagnosticsResponse>>> Get()
        {
            var items = await _service.GetAllDiagnosticss();
            var response = items.Select(i => new DiagnosticsResponse(i.Id, i.DiagnosticsDate, i.ResultDescription, i.EmployeeId, i.RequestId));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<DiagnosticsResponse>> GetById(Guid id)
        {
            var item = await _service.GetDiagnosticsById(id);
            var response = new DiagnosticsResponse(item.Id, item.DiagnosticsDate, item.ResultDescription, item.EmployeeId, item.RequestId);
            return Ok(response);
        }

        [HttpGet("request/{requestId:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<List<DiagnosticsResponse>>> GetByRequestId(Guid requestId)
        {
            var items = await _service.GetDiagnosticsByRequestId(requestId);
            var response = items.Select(i => new DiagnosticsResponse(i.Id, i.DiagnosticsDate, i.ResultDescription, i.EmployeeId, i.RequestId));
            return Ok(response);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<Guid>> Create([FromBody] DiagnosticsRequest request)
        {
            var userId = User.FindFirst("userId")?.Value;

            if (Guid.TryParse(userId, out var userGuid))
            {
                var employeeId = await _userService.GetEmployeeIdByUserId(userGuid);

                if (employeeId == Guid.Empty)
                {
                    return BadRequest("Employee ID not found for the given user.");
                }

                var (model, error) = Diagnostics.Create(Guid.NewGuid(), request.DiagnosticsDate, request.ResultDescription, employeeId, request.RequestId);
                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }

                var id = await _service.CreateDiagnostics(model);
                return Ok(id);
            }

            return Unauthorized("Invalid user ID.");
        }


        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] DiagnosticsUpdate request)
        {
            var updated = await _service.UpdateDiagnostics(id, request.DiagnosticsDate, request.ResultDescription, request.EmployeeId, request.RequestId);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteDiagnostics(id);
            return Ok(id);
        }
    }
}