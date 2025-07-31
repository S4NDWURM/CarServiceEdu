namespace CarService.Core.Models
{
    public class CarBrand
    {
        private CarBrand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }

        public static (CarBrand? Item, string Error) Create(Guid id, string name)
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

            var item = new CarBrand(id, name);
            return (item, string.Empty);
        }
    }
}