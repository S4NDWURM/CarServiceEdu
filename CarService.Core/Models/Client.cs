namespace CarService.Core.Models
{
    public class Client
    {
        private Client(Guid id, string lastName, string firstName, string middleName, DateTime dateOfBirth, DateTime registrationDate)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            DateOfBirth = dateOfBirth;
            RegistrationDate = registrationDate;
        }

        public Guid Id { get; }
        public string LastName { get; }
        public string FirstName { get; }
        public string MiddleName { get; }
        public DateTime DateOfBirth { get; }
        public DateTime RegistrationDate { get; }

        public static (Client? Item, string Error) Create(Guid id, string lastName, string firstName, string middleName, DateTime dateOfBirth, DateTime registrationDate)
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

            if (dateOfBirth > DateTime.Now)
            {
                return (null, "Date of birth cannot be in the future.");
            }

            if (DateTime.Now.Year - dateOfBirth.Year < 18)
            {
                return (null, "Client must be at least 18 years old.");
            }

            if (registrationDate < dateOfBirth)
            {
                return (null, "Registration date cannot be earlier than date of birth.");
            }

            var item = new Client(id, lastName, firstName, middleName, dateOfBirth, registrationDate);
            return (item, string.Empty);
        }
    }
}
