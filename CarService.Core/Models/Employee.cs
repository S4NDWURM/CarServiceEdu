namespace CarService.Core.Models
{
    public class Employee
    {
        private Employee(Guid id, string lastName, string firstName, string middleName, int workExperience, DateTime hireDate, Guid employeeStatus)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            WorkExperience = workExperience;
            HireDate = hireDate;
            EmployeeStatus = employeeStatus;
        }

        public Guid Id { get; }
        public string LastName { get; }
        public string FirstName { get; }
        public string MiddleName { get; }
        public int WorkExperience { get; }
        public DateTime HireDate { get; }
        public Guid EmployeeStatus { get; }

        public static (Employee? Item, string Error) Create(Guid id, string lastName, string firstName, string middleName, int workExperience, DateTime hireDate, Guid employeeStatus)
        {
            if (id == Guid.Empty)
            {
                return (null, "Id cannot be empty.");
            }

            if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(middleName))
            {
                return (null, "Full name cannot be null or empty.");
            }

            if (lastName.Length > 100 || firstName.Length > 100 || middleName.Length > 100)
            {
                return (null, "Name components cannot exceed 100 characters.");
            }

            if (workExperience < 0)
            {
                return (null, "Work experience cannot be negative.");
            }

            if (employeeStatus == Guid.Empty)
            {
                return (null, "Employee status cannot be empty.");
            }

            var item = new Employee(id, lastName, firstName, middleName, workExperience, hireDate, employeeStatus);
            return (item, string.Empty);
        }
    }
}
