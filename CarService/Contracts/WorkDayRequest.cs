namespace CarService.API.Contracts
{
    public record WorkDayRequest(
        Guid EmployeeId,
        Guid TypeOfDayId,
        TimeSpan StartTime,
        TimeSpan EndTime);
}