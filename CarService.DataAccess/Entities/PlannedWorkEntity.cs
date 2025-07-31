namespace CarService.DataAccess.Entities
{
    public class PlannedWorkEntity
    {
        public Guid Id { get; set; }
        public DateTime PlanDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public decimal TotalCost { get; set; }
        public Guid WorkId { get; set; }
        public Guid RequestId { get; set; }   
        public Guid StatusId { get; set; }
        public WorkEntity Work { get; set; }
        public RequestEntity Request { get; set; }
        public StatusEntity Status { get; set; }

        public ICollection<PlannedWorkPartEntity> PlannedWorkParts { get; set; } = [];
        public ICollection<PlannedWorkEmployeeEntity> PlannedWorkEmployees { get; set; } = [];

    }
}