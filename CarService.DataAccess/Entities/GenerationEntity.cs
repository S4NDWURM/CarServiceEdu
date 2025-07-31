namespace CarService.DataAccess.Entities
{
    public class GenerationEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public Guid CarModelId { get; set; }         
        public CarModelEntity CarModel { get; set; }
        public ICollection<VehicleEntity> Vehicles { get; set; } = [];
    }
}
