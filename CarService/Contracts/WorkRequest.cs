namespace CarService.API.Contracts
{
    public record WorkRequest(
        string Name,
        string Description,
        decimal Cost);
}