using CarService.API.Contracts;
using CarService.Application.Services;
using CarService.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _service;
        private readonly IUserService _userService;
        public ClientsController(IClientService service, IUserService userService)
        {
            _service = service;
            _userService = userService;

        }

        [HttpGet("me")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<ClientResponse>> GetMe()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (Guid.TryParse(userId, out var userGuid))
            {
                var clientId = await _userService.GetClientIdByUserId(userGuid);
                var item = await _service.GetClientById(clientId);
                if (item == null)
                {
                    return NotFound($"Client with id {clientId} not found");
                }

                var response = new ClientResponse(
                    item.Id, item.LastName, item.FirstName, item.MiddleName, item.DateOfBirth, item.RegistrationDate);
                return Ok(response);
            }

            return Unauthorized("Invalid user ID.");
        }

        [HttpPut("me")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<Guid>> UpdateMe([FromBody] ClientRequest request)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (Guid.TryParse(userId, out var userGuid))
            {
                Guid clientId = await _userService.GetClientIdByUserId(userGuid);
                var updated = await _service.UpdateClient(clientId, request.LastName, request.FirstName, request.MiddleName,
                    request.DateOfBirth, request.RegistrationDate);
                return Ok(updated);
            }

            return Unauthorized("Invalid user ID.");
        }

        [HttpDelete("me")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<Guid>> DeleteMe()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (Guid.TryParse(userId, out var userGuid))
            {
                var clientId = await _userService.GetClientIdByUserId(userGuid);
                await _service.DeleteClient(clientId);
                return Ok(clientId);
            }

            return Unauthorized("Invalid user ID.");
        }

        [HttpGet]
        [Authorize(Roles = "Specialist, Admin")]
        public async Task<ActionResult<List<ClientResponse>>> Get()
        {
            var items = await _service.GetAllClients();
            var response = items.Select(i => new ClientResponse(
                i.Id, i.LastName, i.FirstName, i.MiddleName, i.DateOfBirth, i.RegistrationDate));
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Specialist,Admin")]
        public async Task<ActionResult<ClientResponse>> GetById(Guid id)
        {
            var item = await _service.GetClientById(id);
            if (item == null)
            {
                return NotFound($"Client with id {id} not found");
            }

            var response = new ClientResponse(item.Id, item.LastName, item.FirstName, item.MiddleName,
                item.DateOfBirth, item.RegistrationDate);
            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] ClientRequest request)
        {
            var updated = await _service.UpdateClient(id, request.LastName, request.FirstName, request.MiddleName,
                request.DateOfBirth, request.RegistrationDate);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _service.DeleteClient(id);
            return Ok(id);
        }
    }
}
