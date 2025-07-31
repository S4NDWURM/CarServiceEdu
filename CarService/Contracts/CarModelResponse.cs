namespace CarService.API.Contracts
{
    public record CarModelResponse(
        Guid Id,
        string Name,
        Guid CarBrandId);
}