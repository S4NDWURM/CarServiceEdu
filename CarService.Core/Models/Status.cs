namespace CarService.Core.Models
{
    public class Status
    {
        private Status(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }

        public static (Status? Item, string Error) Create(Guid id, string name)
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

            var item = new Status(id, name);
            return (item, error);
        }
    }
}
