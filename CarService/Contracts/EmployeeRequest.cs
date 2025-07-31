namespace CarService.API.Contracts
{
    public record EmployeeRequest(
        string LastName,
        string FirstName,
        string MiddleName,
        int WorkExperience, 
        DateTime HireDate,  
        Guid EmployeeStatus);
}