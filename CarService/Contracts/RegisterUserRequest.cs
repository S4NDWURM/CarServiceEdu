using System.ComponentModel.DataAnnotations;

namespace CarService.API.Contracts
{
    public record RegisterUserRequest(
        [Required] string UserName,
        [Required] string Password,
        [Required] string Email,
        [Required] Guid RoleId,
        [Required] Guid ClientId, 
        [Required] Guid EmployeeId); 
}
