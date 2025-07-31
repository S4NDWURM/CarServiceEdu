using System.ComponentModel.DataAnnotations;

namespace CarService.API.Contracts
{
    public record LoginUserRequest(
        [Required] string Email,
        [Required] string Password);
}
