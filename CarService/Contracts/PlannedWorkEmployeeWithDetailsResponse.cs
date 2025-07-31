namespace CarService.API.Contracts
{
    public class PlannedWorkEmployeeWithDetailsResponse
    {
        public Guid PlannedWorkId { get; }
        public Guid EmployeeId { get; }
        public string LastName { get; }
        public string FirstName { get; }
        public string MiddleName { get; }

        public PlannedWorkEmployeeWithDetailsResponse(Guid plannedWorkId, Guid employeeId, string lastName, string firstName, string middleName)
        {
            PlannedWorkId = plannedWorkId;
            EmployeeId = employeeId;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
        }
    }
}
