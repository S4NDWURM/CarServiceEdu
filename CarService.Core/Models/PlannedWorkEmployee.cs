namespace CarService.Core.Models
{
    public class PlannedWorkEmployee
    {
        private PlannedWorkEmployee(Guid plannedWorkId, Guid employeeId)
        {
            PlannedWorkId = plannedWorkId;
            EmployeeId = employeeId;
        }

        public Guid PlannedWorkId { get; }
        public Guid EmployeeId { get; }

        public static (PlannedWorkEmployee? Item, string Error) Create(Guid plannedWorkId, Guid employeeId)
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

            var item = new PlannedWorkEmployee(plannedWorkId, employeeId);
            return (item, string.Empty);
        }
    }
}
