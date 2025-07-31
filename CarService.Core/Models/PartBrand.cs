namespace CarService.Core.Models
{
    public class PartBrand
    {
        private PartBrand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }

        public static (PartBrand? Item, string Error) Create(Guid id, string name)
        {
            string error = string.Empty;

            if (id == Guid.Empty)
            {
                return (null, "Id cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return (null, "Name cannot be null or empty.");
            }

            if (name.Length > 100)
            {
                return (null, "Name cannot exceed 100 characters.");
            }

            var item = new PartBrand(id, name);
            return (item, string.Empty);
        }
    }
}
