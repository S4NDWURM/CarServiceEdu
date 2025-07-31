using System.ComponentModel.DataAnnotations;

namespace CarService.API.Contracts
{
    public record RegisterClientRequest(
        [Required] string UserName,
        [Required] string Password,
        [Required] string Email,  
        [Required] string LastName,
        [Required] string FirstName,
        [Required] string MiddleName,
        [Required] DateTime DateOfBirth);
}
