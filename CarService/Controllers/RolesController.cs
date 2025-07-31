using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarService.API.Contracts;
using CarService.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace CarService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin, Specialist")]
        public async Task<IResult> GetAll(
            [FromServices] IRoleService roleService)
        {
            var roles = await roleService.GetAll();
            var dto = roles.Select(r => new RoleRequestResponse(r.Id, r.Name));
            return Results.Ok(dto);
        }
    }
}
