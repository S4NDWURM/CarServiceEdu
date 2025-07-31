namespace CarService.DataAccess.Entities
{
    public class StatusEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<RequestEntity> Requests { get; set; } = [];
        public ICollection<PlannedWorkEntity> PlannedWorks { get; set; } = [];
    }
}