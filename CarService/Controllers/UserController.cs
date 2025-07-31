using CarService.API.Contracts;
using CarService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register-client")]
        public async Task<IResult> RegisterClient(RegisterClientRequest request)
        {
            await _userService.RegisterClient(request.UserName, request.Email, request.Password, Guid.Parse("33333333-3333-3333-3333-333333333333"), request.LastName, request.FirstName, request.MiddleName, request.DateOfBirth, DateTime.UtcNow);
            return Results.Ok();
        }

        [HttpPost("register-employee")]
        [Authorize(Roles = "Admin")]
        public async Task<IResult> RegisterEmployee(RegisterEmployeeRequest request)
        {
            await _userService.RegisterEmployee(request.UserName, request.Email, request.Password, Guid.Parse("22222222-2222-2222-2222-222222222222"),
                request.EmployeeStatusId, request.LastName, request.FirstName, request.MiddleName, request.WorkExperience, request.HireDate);
            return Results.Ok();
        }

        [HttpPost("login")]
        public async Task<IResult> Login(LoginUserRequest request)
        {
            var token = await _userService.Login(request.Email, request.Password);
            Response.Cookies.Append("jwt-token", token);
            return Results.Ok(token);
        }

        [HttpGet("get-role")]
        [Authorize(Roles = "Admin,Client,Specialist")]
        public IResult GetRole()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return Results.Ok(role);
        }

        [HttpPost("logout")]
        [Authorize]
        public IResult Logout()
        {
            Response.Cookies.Delete("jwt-token");
            return Results.Ok("Вы успешно вышли из системы");
        }
    }
}
