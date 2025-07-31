namespace CarService.DataAccess.Entities
{
    public class PlannedWorkEmployeeEntity
    {
        public Guid PlannedWorkId { get; set; }
        public Guid EmployeeId { get; set; }

        public PlannedWorkEntity PlannedWork { get; set; }
        public EmployeeEntity Employee { get; set; }
    }
}