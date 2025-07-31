namespace CarService.Core.Models
{
    public class Role
    {
        private Role(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }

        public static (Role? Item, string Error) Create(Guid id, string name)
        {
            var error = string.Empty;

            if (id == Guid.Empty)
            {
                return (null, "Role ID cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return (null, "Role name cannot be empty.");
            }

            if (name.Length > 100)
            {
                return (null, "Role name cannot exceed 100 characters.");
            }

            var item = new Role(id, name);
            return (item, error);
        }
    }
}
