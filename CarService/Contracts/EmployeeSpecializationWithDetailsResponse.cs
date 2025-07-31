namespace CarService.API.Contracts
{
    public class EmployeeSpecializationWithDetailsResponse
    {
        public Guid EmployeeId { get; }
        public Guid SpecializationId { get; }
        public string Name { get; }
        public string Description { get; }

        public EmployeeSpecializationWithDetailsResponse(Guid employeeId, Guid specializationId, string name, string description)
        {
            EmployeeId = employeeId;
            SpecializationId = specializationId;
            Name = name;
            Description = description;
        }
    }
}
