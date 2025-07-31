namespace CarService.API.Contracts
{
    public record PlannedWorkEmployeeRequest(
        Guid PlannedWorkId,
        Guid EmployeeId);
}