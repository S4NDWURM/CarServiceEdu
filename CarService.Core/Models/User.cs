using System.Text.RegularExpressions;

namespace CarService.Core.Models
{
    public class User
    {
        private User(Guid id, string userName, string passwordHash, string email, Guid roleId, Guid? clientId, Guid? employeeId)
        {
            Id = id;
            UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
            RoleId = roleId;
            ClientId = clientId;
            EmployeeId = employeeId;
        }

        public Guid Id { get; }
        public string UserName { get; }
        public string PasswordHash { get; }
        public string Email { get; }
        public Guid RoleId { get; }
        public Guid? ClientId { get; set; }  
        public Guid? EmployeeId { get; set; }

        public static (User? Item, string Error) Create(Guid id, string userName, string passwordHash, string email, Guid roleId, Guid? clientId, Guid? employeeId)
        {
            string error = string.Empty;

            if (id == Guid.Empty)
            {
                return (null, "Id cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                return (null, "UserName cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                return (null, "Password cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                return (null, "Invalid email format.");
            }

            if (roleId == Guid.Empty)
            {
                return (null, "RoleId cannot be empty.");
            }
            var item = new User(id, userName, passwordHash, email, roleId, clientId, employeeId);
            return (item, error);
        }

        private static bool IsValidEmail(string email)
        {
            var emailRegex = @"^[a-zA-Z0-9_+&*-]+(?:\.[a-zA-Z0-9_+&*-]+)*@" +
                             @"(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,7}$";
            var regex = new Regex(emailRegex);
            return regex.IsMatch(email);
        }
    }
}
