namespace CarService.API.Contracts
{
    public record DiagnosticsUpdate(
        DateTime DiagnosticsDate,
        string ResultDescription,
        Guid EmployeeId,
        Guid RequestId);
}