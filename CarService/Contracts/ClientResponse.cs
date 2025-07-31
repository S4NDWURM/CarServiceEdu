namespace CarService.API.Contracts
{
    public record ClientResponse(
        Guid Id,
        string LastName,
        string FirstName,
        string MiddleName,
        DateTime DateOfBirth,
        DateTime RegistrationDate);
}