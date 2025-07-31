namespace CarService.Core.Models
{
    public class WorkDay
    {
        private WorkDay(Guid id, Guid employeeId, Guid typeOfDayId, TimeSpan startTime, TimeSpan endTime)
        {
            Id = id;
            EmployeeId = employeeId;
            TypeOfDayId = typeOfDayId;
            StartTime = startTime;
            EndTime = endTime;
        }

        public Guid Id { get; }
        public Guid EmployeeId { get; }
        public Guid TypeOfDayId { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }

        public static (WorkDay? Item, string Error) Create(Guid id, Guid employeeId, Guid typeOfDayId, TimeSpan startTime, TimeSpan endTime)
        {
            string error = string.Empty;

            if (id == Guid.Empty)
            {
                return (null, "Id cannot be empty.");
            }

            if (employeeId == Guid.Empty)
            {
                return (null, "Employee ID cannot be empty.");
            }

            if (typeOfDayId == Guid.Empty)
            {
                return (null, "Type of Day ID cannot be empty.");
            }

            if (startTime >= endTime)
            {
                return (null, "Start time cannot be later than or equal to end time.");
            }

            var item = new WorkDay(id, employeeId, typeOfDayId, startTime, endTime);
            return (item, error);
        }
    }
}
