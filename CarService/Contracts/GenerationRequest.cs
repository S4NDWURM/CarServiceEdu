namespace CarService.API.Contracts
{
    public record GenerationRequest(
        Guid CarModelId,
        string Name,
        int StartYear,
        int EndYear);
}
