namespace CarService.API.Contracts
{
    public record ClientRequest(
        string LastName,
        string FirstName,
        string MiddleName,
        DateTime DateOfBirth,
        DateTime RegistrationDate);
}