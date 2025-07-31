namespace CarService.API.Contracts
{
    public record UserRequestResponse(
        Guid Id,
        string Reason,
        DateTime OpenDate,
        DateTime? CloseDate,
        Guid ClientId,
        Guid VehicleId,
        Guid StatusId);
}