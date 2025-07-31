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
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _service;
        private readonly IUserService _userService;
        private readonly IPartService _partService;
        private readonly IWorkService _workService;
        private readonly IExcelGenerationService _excelGenerationService;
        private readonly IPlannedWorkService _plannedWorkService;
        private readonly IPlannedWorkPartService _plannedWorkPartService;

        public RequestsController(IRequestService service, IUserService userService, IPartService partService, IWorkService workService, IExcelGenerationService excelGenerationService, IPlannedWorkService plannedWorkService, IPlannedWorkPartService plannedWorkPartService)
        {
            _service = service;
            _userService = userService;
            _partService = partService;
            _workService = workService;
            _excelGenerationService = excelGenerationService;
            _plannedWorkService = plannedWorkService;
            _plannedWorkPartService = plannedWorkPartService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Specialist")]
        public async Task<ActionResult<List<UserRequestResponse>>> Get()
        {
            var items = await _service.GetAllRequests();
            var response = items.Select(i => new UserRequestResponse(i.Id, i.Reason, i.OpenDate, i.CloseDate, i.ClientId, i.VehicleId, i.StatusId));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Specialist")]
        public async Task<ActionResult<UserRequestResponse>> GetById(Guid id)
        {
            var item = await _service.GetRequestById(id);
            if (item == null)
            {
                return NotFound($"Request with id {id} not found");
            }

            var response = new UserRequestResponse(item.Id, item.Reason, item.OpenDate, item.CloseDate, item.ClientId, item.VehicleId, item.StatusId);
            return Ok(response);
        }

        [HttpGet("details/{id:guid}")]
        [Authorize(Roles = "Admin, Specialist")]
        public async Task<ActionResult<UserRequestWithDetailsResponse>> GetByIdDetailed(Guid id)
        {
            var item = await _service.GetRequestByIdDetailed(id);
            if (item == null)
            {
                return NotFound($"Request with id {id} not found");
            }

            var response = new UserRequestWithDetailsResponse(
                item.Id,
                item.Reason,
                item.OpenDate,
                item.CloseDate,
                new UserRequestClientResponse(item.Client.Id, item.Client.LastName, item.Client.FirstName, item.Client.MiddleName, item.Client.Email),
                new UserRequestVehicleResponse(item.Vehicle.Id, item.Vehicle.VIN, item.Vehicle.Year),
                new UserRequestStatusResponse(item.Status.Id, item.Status.Name)
            );
            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<List<UserRequestResponse>>> GetRequestsForClient()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (Guid.TryParse(userId, out var userGuid))
            {
                var clientId = await _userService.GetClientIdByUserId(userGuid);
                var items = await _service.GetRequestsByClientId(clientId);

                var response = items.Select(i => new UserRequestResponse(
                    i.Id, i.Reason, i.OpenDate, i.CloseDate, i.ClientId, i.VehicleId, i.StatusId));
                return Ok(response);
            }

            return Unauthorized("Invalid user ID.");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Client")]
        public async Task<ActionResult<Guid>> Create([FromBody] UserRequestRequest request)
        {
            var userId = User.FindFirst("userId")?.Value;

            if (Guid.TryParse(userId, out var userGuid))
            {
                var clientId = await _userService.GetClientIdByUserId(userGuid);

                if (clientId == Guid.Empty)
                {
                    return BadRequest("Client ID not found for the given user.");
                }

                var (model, error) = UserRequest.Create(Guid.NewGuid(), request.Reason, DateTime.UtcNow, request.CloseDate, clientId, request.VehicleId, Guid.Parse("11111111-1111-1111-1111-111111111111"));

                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }

                var id = await _service.CreateRequest(model);
                return Ok(id);
            }

            return Unauthorized("Invalid user ID.");
        }


        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Specialist")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] UserRequestUpdate request)
        {
            var updated = await _service.UpdateRequest(id, request.Reason, request.OpenDate, request.CloseDate, request.ClientId, request.VehicleId, request.StatusId);
            return Ok(updated);
        }

        [HttpPut("{id:guid}/status")]
        [Authorize(Roles = "Admin, Specialist")]
        public async Task<ActionResult<Guid>> UpdateStatus(Guid id, [FromBody] UserRequestStatusUpdate request)
        {
            var updatedRequestId = await _service.UpdateRequestStatus(id, request.StatusId);

            if (updatedRequestId == Guid.Empty)
            {
                return NotFound($"Request with id {id} not found.");
            }

            return Ok(updatedRequestId);
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteRequest(id);
            return Ok(id);
        }

        [HttpGet("{requestId:guid}/generate-excel")]
        [Authorize(Roles = "Admin, Specialist")]
        public async Task<IActionResult> GenerateOrderRequestExcel(Guid requestId)
        {
            var request = await _service.GetRequestById(requestId);
            if (request == null)
            {
                return NotFound($"Заявка с id = {requestId} не найдена");
            }

            var plannedWorks = await _plannedWorkService.GetPlannedWorksByRequestId(requestId);

            var works = new List<Work>();
            foreach (var plannedWork in plannedWorks)
            {
                var work = await _workService.GetWorkById(plannedWork.WorkId);
                works.Add(work);
            }

            var parts = new List<Part>();
            var partQuantities = new List<int>(); 
            foreach (var plannedWork in plannedWorks)
            {
                var plannedWorkParts = await _plannedWorkPartService.GetPlannedWorkPartsById(plannedWork.Id);
                foreach (var plannedWorkPart in plannedWorkParts)
                {
                    var part = await _partService.GetPartById(plannedWorkPart.PartId);
                    parts.Add(part);
                    partQuantities.Add(plannedWorkPart.Quantity); 
                }
            }

            var excelFileStream = await _excelGenerationService.GenerateOrderRequestExcel(requestId, parts, works, partQuantities);
            return File(excelFileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"OrderRequest_{requestId}.xlsx");
        }



    }
}