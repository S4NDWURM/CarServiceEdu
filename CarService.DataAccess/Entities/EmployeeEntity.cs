namespace CarService.DataAccess.Entities
{
    public class EmployeeEntity
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int WorkExperience { get; set; } 
        public DateTime HireDate { get; set; }   
        public Guid EmployeeStatusId { get; set; }
        public EmployeeStatusEntity EmployeeStatus { get; set; }
        public UserEntity User { get; set; }

        public ICollection<PlannedWorkEmployeeEntity> PlannedWorkEmployees { get; set; } = [];
        public ICollection<EmployeeSpecializationEntity> EmployeeSpecializations { get; set; } = [];
        public ICollection<DiagnosticsEntity> Diagnostics { get; set; } = [];
        public ICollection<WorkDayEntity> WorkDays { get; set; } = [];
    }
}
