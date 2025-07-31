namespace CarService.DataAccess.Entities
{
    public class TypeOfDayEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<WorkDayEntity> WorkDays { get; set; } = [];
    }
}