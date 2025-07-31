namespace CarService.API.Contracts
{
    public record VehicleRequest(
        string VIN,
        int Year,
        Guid GenerationId);
}