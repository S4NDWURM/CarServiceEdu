namespace CarService.DataAccess.Entities
{
    public class PlannedWorkPartEntity
    {
        public Guid PlannedWorkId { get; set; }
        public Guid PartId { get; set; }
        public int Quantity { get; set; }

        public PlannedWorkEntity PlannedWork { get; set; }
        public PartEntity Part { get; set; }
    }
}