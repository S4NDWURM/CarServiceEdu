namespace CarService.API.Contracts
{
    public record GenerationResponse(
        Guid Id,
        Guid CarModelId,
        string Name,
        int StartYear,
        int EndYear);
}
