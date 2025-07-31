namespace CarService.API.Contracts
{
    public record PlannedWorkPartRequest(
        Guid PlannedWorkId,
        Guid PartId,
        int Quantity);
}