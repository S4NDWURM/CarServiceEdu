namespace CarService.API.Contracts
{
    public record PartResponse(
        Guid Id,
        string Name,
        string Article,
        decimal Cost,
        Guid PartBrandId);
}