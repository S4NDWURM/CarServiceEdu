namespace CarService.API.Contracts
{
    public record WorkResponse(
        Guid Id,
        string Name,
        string Description,
        decimal Cost);
}