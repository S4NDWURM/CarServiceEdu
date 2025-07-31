namespace CarService.API.Contracts
{
    public record DiagnosticsRequest(
        DateTime DiagnosticsDate,
        string ResultDescription,
        Guid RequestId);
}