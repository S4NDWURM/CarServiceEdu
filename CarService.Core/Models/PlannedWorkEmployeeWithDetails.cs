namespace CarService.Core.Models
{
    public class PlannedWorkEmployeeWithDetails
    {
        public Guid PlannedWorkId { get; }
        public Guid EmployeeId { get; }
        public string LastName { get; }
        public string FirstName { get; }
        public string MiddleName { get; }

        private PlannedWorkEmployeeWithDetails(Guid plannedWorkId, Guid employeeId, string lastName, string firstName, string middleName)
        {
            PlannedWorkId = plannedWorkId;
            EmployeeId = employeeId;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
        }

        public static (PlannedWorkEmployeeWithDetails? Item, string Error) Create(Guid plannedWorkId, Guid employeeId, string lastName, string firstName, string middleName)
        {
            string error = string.Empty;

            if (plannedWorkId == Guid.Empty)
            {
                return (null, "Planned work ID cannot be empty.");
            }

            if (employeeId == Guid.Empty)
            {
                return (null, "Employee ID cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                return (null, "Employee last name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                return (null, "Employee first name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(middleName))
            {
                return (null, "Employee middle name cannot be empty.");
            }

            if (lastName.Length > 100 || firstName.Length > 100 || middleName.Length > 100)
            {
                return (null, "Name components cannot exceed 100 characters.");
            }

            var item = new PlannedWorkEmployeeWithDetails(plannedWorkId, employeeId, lastName, firstName, middleName);
            return (item, string.Empty);
        }
    }
}
