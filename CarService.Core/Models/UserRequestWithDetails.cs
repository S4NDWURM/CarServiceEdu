using System.Text.RegularExpressions;

namespace CarService.Core.Models
{
    public class UserRequestWithDetailsModel
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public UserClientModel Client { get; set; }
        public UserVehicleModel Vehicle { get; set; }
        public UserStatusModel Status { get; set; }

        public static (UserRequestWithDetailsModel? Item, string Error) Create(
            Guid id,
            string reason,
            DateTime openDate,
            DateTime? closeDate,
            UserClientModel client,
            UserVehicleModel vehicle,
            UserStatusModel status)
        {

            var error = string.Empty;

            if (string.IsNullOrEmpty(reason))
            {
                error = "Reason cannot be empty.";
            }
            else if (client == null)
            {
                error = "Client information is missing.";
            }
            else if (vehicle == null)
            {
                error = "Vehicle information is missing.";
            }
            else if (status == null)
            {
                error = "Status information is missing.";
            }
            else if (string.IsNullOrEmpty(client.Email) || !IsValidEmail(client.Email))
            {
                error = "Invalid email address.";
            }
            else if (string.IsNullOrEmpty(vehicle.VIN) || !IsValidVIN(vehicle.VIN))
            {
                error = "Invalid VIN. It must be exactly 17 characters long and contain only letters and digits.";
            }

            UserRequestWithDetailsModel? item = null;
            if (string.IsNullOrEmpty(error))
            {
                item = new UserRequestWithDetailsModel
                {
                    Id = id,
                    Reason = reason,
                    OpenDate = openDate,
                    CloseDate = closeDate,
                    Client = client,
                    Vehicle = vehicle,
                    Status = status
                };
            }

            return (item, error);
        }

        private static bool IsValidVIN(string vin)
        {
            var vinRegex = @"(?=.*\d|=.*[A-Z])(?=.*[A-Z])[A-Z0-9]{17}";
            var regex = new Regex(vinRegex);
            return regex.IsMatch(vin);
        }

        private static bool IsValidEmail(string email)
        {
            var emailRegex = @"^[a-zA-Z0-9_+&*-]+(?:\.[a-zA-Z0-9_+&*-]+)*@" +
                             @"(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,7}$";
            var regex = new Regex(emailRegex);
            return regex.IsMatch(email);
        }
    }

    public class UserClientModel
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
    }

    public class UserVehicleModel
    {
        public Guid Id { get; set; }
        public string VIN { get; set; }
        public int Year { get; set; }
    }

    public class UserStatusModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}