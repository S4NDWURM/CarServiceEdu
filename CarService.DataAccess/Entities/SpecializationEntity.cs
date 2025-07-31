namespace CarService.DataAccess.Entities
{
    public class SpecializationEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<EmployeeSpecializationEntity> EmployeeSpecializations { get; set; } = [];
    }
}