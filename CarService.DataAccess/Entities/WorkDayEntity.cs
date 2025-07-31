namespace CarService.DataAccess.Entities
{
    public class WorkDayEntity
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid TypeOfDayId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public EmployeeEntity Employee { get; set; }
        public TypeOfDayEntity TypeOfDay { get; set; }   
    }
}