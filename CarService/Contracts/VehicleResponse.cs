namespace CarService.API.Contracts
{
    public record VehicleResponse(
        Guid Id,
        string VIN,
        int Year,
        Guid GenerationId);
}
