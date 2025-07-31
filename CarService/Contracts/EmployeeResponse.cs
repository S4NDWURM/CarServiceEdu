namespace CarService.API.Contracts
{
    public record EmployeeResponse(
        Guid Id,
        string LastName,
        string FirstName,
        string MiddleName,
        int WorkExperience, 
        DateTime HireDate,   
        Guid EmployeeStatus); 
}