namespace CarService.API.Contracts
{
    public record WorkDayResponse(
        Guid Id,
        Guid EmployeeId,
        Guid TypeOfDayId,
        TimeSpan StartTime,
        TimeSpan EndTime);
}