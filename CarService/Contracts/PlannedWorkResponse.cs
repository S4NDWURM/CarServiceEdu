namespace CarService.API.Contracts
{
    public record PlannedWorkResponse(
        Guid Id,
        DateTime PlanDate,
        DateTime ExpectedEndDate,
        decimal TotalCost,
        Guid WorkId,
        Guid RequestId,
        Guid StatusId);
}