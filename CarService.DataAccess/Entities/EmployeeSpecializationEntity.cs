namespace CarService.DataAccess.Entities
{
    public class EmployeeSpecializationEntity
    {
        public Guid EmployeeId { get; set; }
        public Guid SpecializationId { get; set; }

        public EmployeeEntity Employee { get; set; } 
        public SpecializationEntity Specialization { get; set; } 
    }
}