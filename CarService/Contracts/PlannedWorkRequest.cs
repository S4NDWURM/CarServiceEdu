namespace CarService.API.Contracts
{
    public record PlannedWorkRequest(
        DateTime PlanDate,
        DateTime ExpectedEndDate,
        decimal TotalCost,
        Guid WorkId,
        Guid RequestId,
        Guid StatusId);
}