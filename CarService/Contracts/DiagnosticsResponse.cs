namespace CarService.API.Contracts
{
    public record DiagnosticsResponse(
        Guid Id,
        DateTime DiagnosticsDate,
        string ResultDescription,
        Guid EmployeeId,
        Guid RequestId);
}