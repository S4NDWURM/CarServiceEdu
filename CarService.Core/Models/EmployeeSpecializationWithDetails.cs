namespace CarService.Core.Models
{
    public class EmployeeSpecializationWithDetails
    {
        public Guid EmployeeId { get; }
        public Guid SpecializationId { get; }
        public string Name { get; }
        public string Description { get; }


        private EmployeeSpecializationWithDetails(Guid employeeId, Guid specializationId, string name, string description)
        {
            EmployeeId = employeeId;
            SpecializationId = specializationId;
            Name = name;
            Description = description;
        }


        public static (EmployeeSpecializationWithDetails? Item, string Error) Create(Guid employeeId, Guid specializationId, string name, string description)
        {
            string error = string.Empty;

            if (employeeId == Guid.Empty)
            {
                return (null, "Employee ID cannot be empty.");
            }


            if (specializationId == Guid.Empty)
            {
                return (null, "Specialization ID cannot be empty.");
            }


            if (string.IsNullOrEmpty(name))
            {
                return (null, "Specialization name cannot be empty.");
            }


            if (name.Length > 100)
            {
                return (null, "Specialization name cannot exceed 100 characters.");
            }


            if (string.IsNullOrEmpty(description))
            {
                return (null, "Specialization description cannot be empty.");
            }


            if (description.Length > 1000)
            {
                return (null, "Specialization description cannot exceed 1000 characters.");
            }


            var item = new EmployeeSpecializationWithDetails(employeeId, specializationId, name, description);
            return (item, string.Empty);
        }
    }
}
