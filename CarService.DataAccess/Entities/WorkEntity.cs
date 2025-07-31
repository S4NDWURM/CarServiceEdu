namespace CarService.DataAccess.Entities
{
    public class WorkEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public ICollection<PlannedWorkEntity> PlannedWorks { get; set; } = [];
    }
}