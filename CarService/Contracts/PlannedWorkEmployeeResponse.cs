namespace CarService.API.Contracts
{
    public record PlannedWorkEmployeeResponse(
        Guid PlannedWorkId,
        Guid EmployeeId);
}