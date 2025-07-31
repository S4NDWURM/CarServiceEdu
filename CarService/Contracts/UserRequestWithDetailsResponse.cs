namespace CarService.API.Contracts
{
    public record UserRequestWithDetailsResponse(
        Guid Id,
        string Reason,
        DateTime OpenDate,
        DateTime? CloseDate,
        UserRequestClientResponse Client,     
        UserRequestVehicleResponse Vehicle,   
        UserRequestStatusResponse Status      
    );

    public record UserRequestClientResponse(
        Guid Id,
        string LastName,
        string FirstName,
        string MiddleName,
        string Email
    );

    public record UserRequestVehicleResponse(
        Guid Id,
        string VIN,
        int Year
    );

    public record UserRequestStatusResponse(
        Guid Id,
        string Name
    );
}