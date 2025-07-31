namespace CarService.DataAccess.Entities
{
    public class PartBrandEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<PartEntity> Parts { get; set; } = [];
    }
}