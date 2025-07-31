namespace CarService.API.Contracts
{
    public record CarModelRequest(
        string Name,
        Guid CarBrandId);
}