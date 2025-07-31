namespace CarService.API.Contracts
{
    public record PlannedWorkWithDetailsResponse(
        Guid Id,
        DateTime PlanDate,
        DateTime ExpectedEndDate,
        decimal TotalCost,
        Guid WorkId,
        Guid RequestId,
        Guid StatusId,
        string WorkName,
        string Description,
        decimal Cost,
        string StatusName
        );
}
