namespace CarService.DataAccess.Entities
{
    public class CarBrandEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<CarModelEntity> CarModels { get; set; } = [];
    }
}