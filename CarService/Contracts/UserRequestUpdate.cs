namespace CarService.API.Contracts
{
    public record UserRequestUpdate(
        string Reason,
        DateTime OpenDate,
        DateTime? CloseDate,
        Guid ClientId,
        Guid VehicleId,
        Guid StatusId);
}