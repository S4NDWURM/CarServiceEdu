namespace CarService.Core.Models
{
    public class CarModel
    {
        private CarModel(Guid id, string name, Guid carBrandId)
        {
            Id = id;
            Name = name;
            CarBrandId = carBrandId;

        }

        public Guid Id { get; }
        public string Name { get; }
        public Guid CarBrandId { get; set; }

        public static (CarModel? Item, string Error) Create(Guid id, string name, Guid carBrandId)
        {
            if (id == Guid.Empty)
            {
                return (null, "Id cannot be empty.");
            }

            if (string.IsNullOrEmpty(name))
            {
                return (null, "Name cannot be null or empty.");
            }

            if (name.Length > 100)
            {
                return (null, "Name cannot exceed 100 characters.");
            }

            if (carBrandId == Guid.Empty)
            {
                return (null, "Car brand ID cannot be empty.");
            }

            var item = new CarModel(id, name, carBrandId);
            return (item, string.Empty);
        }
    }
}