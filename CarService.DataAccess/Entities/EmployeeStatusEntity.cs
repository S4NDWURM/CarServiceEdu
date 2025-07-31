namespace CarService.DataAccess.Entities
{
    public class EmployeeStatusEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<EmployeeEntity> Employees { get; set; } = new List<EmployeeEntity>();
    }
}
