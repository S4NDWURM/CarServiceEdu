namespace CarService.DataAccess.Entities
{
    public class CarModelEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CarBrandId { get; set; }

        public CarBrandEntity CarBrand { get; set; }
        public ICollection<GenerationEntity> Generations { get; set; } = [];
    }
}