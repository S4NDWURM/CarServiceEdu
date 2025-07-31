using System.ComponentModel.DataAnnotations;

namespace CarService.API.Contracts
{
    public record RegisterEmployeeRequest(
        [Required] string UserName,
        [Required] string Password,
        [Required] string Email,
        [Required] Guid EmployeeStatusId,
        [Required] string LastName,
        [Required] string FirstName,
        [Required] string MiddleName,
        [Required] int WorkExperience,
        [Required] DateTime HireDate);
}
