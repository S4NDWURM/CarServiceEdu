namespace CarService.API.Contracts
{
    public record EmployeeSpecializationRequest(
        Guid EmployeeId,
        Guid SpecializationId);
}