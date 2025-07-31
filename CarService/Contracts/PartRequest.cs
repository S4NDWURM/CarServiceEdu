namespace CarService.API.Contracts
{
    public record PartRequest(
        string Name,
        string Article,
        decimal Cost,
        Guid PartBrandId);
}