namespace CarService.API.Contracts
{
    public record PlannedWorkPartResponse(
        Guid PlannedWorkId,
        Guid PartId,
        int Quantity);
}