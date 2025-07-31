namespace CarService.DataAccess.Entities
{
    public class PartEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Article { get; set; }
        public decimal Cost { get; set; }
        public Guid PartBrandId { get; set; }
        public PartBrandEntity PartBrand { get; set; }
        public ICollection<PlannedWorkPartEntity> PlannedWorkParts { get; set; } = [];
    }
}