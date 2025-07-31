namespace CarService.API.Contracts
{
    public record UserRequestRequest(
        string Reason,
        DateTime? CloseDate,
        Guid VehicleId);
}