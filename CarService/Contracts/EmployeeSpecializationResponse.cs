namespace CarService.API.Contracts
{
    public record EmployeeSpecializationResponse(
        Guid EmployeeId,
        Guid SpecializationId);
}