namespace CarService.Core.Models
{
    public class EmployeeSpecialization
    {
        private EmployeeSpecialization(Guid employeeId, Guid specializationId)
        {
            EmployeeId = employeeId;
            SpecializationId = specializationId;
        }

        public Guid EmployeeId { get; }
        public Guid SpecializationId { get; }

        public static (EmployeeSpecialization? Item, string Error) Create(Guid employeeId, Guid specializationId)
        {
            if (employeeId == Guid.Empty)
            {
                return (null, "Employee ID cannot be empty.");
            }

            if (specializationId == Guid.Empty)
            {
                return (null, "Specialization ID cannot be empty.");
            }

            var item = new EmployeeSpecialization(employeeId, specializationId);
            return (item, string.Empty);
        }
    }
}
