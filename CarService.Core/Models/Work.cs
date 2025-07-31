namespace CarService.Core.Models
{
    public class Work
    {
        private Work(Guid id, string name, string description, decimal cost)
        {
            Id = id;
            Name = name;
            Description = description;
            Cost = cost;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Cost { get; }

        public static (Work? Item, string Error) Create(Guid id, string name, string description, decimal cost)
        {
            string error = string.Empty;

            if (id == Guid.Empty)
            {
                return (null, "Id cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return (null, "Name cannot be empty.");
            }

            if (name.Length > 100)
            {
                return (null, "Name cannot exceed 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return (null, "Description cannot be empty.");
            }

            if (description.Length > 500)
            {
                return (null, "Description cannot exceed 500 characters.");
            }

            if (cost <= 0)
            {
                return (null, "Cost must be greater than zero.");
            }

            var item = new Work(id, name, description, cost);
            return (item, error);
        }
    }
}
