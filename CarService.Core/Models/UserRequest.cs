namespace CarService.Core.Models
{
    public class UserRequest
    {
        private UserRequest(Guid id, string reason, DateTime openDate, DateTime? closeDate, Guid clientId, Guid vehicleId, Guid statusId)
        {
            Id = id;
            Reason = reason;
            OpenDate = openDate;
            CloseDate = closeDate;
            ClientId = clientId;
            VehicleId = vehicleId;
            StatusId = statusId;
        }

        public Guid Id { get; }
        public string Reason { get; }
        public DateTime OpenDate { get; }
        public DateTime? CloseDate { get; }
        public Guid ClientId { get; }
        public Guid VehicleId { get; }
        public Guid StatusId { get; }

        public static (UserRequest? Item, string Error) Create(Guid id, string reason, DateTime openDate, DateTime? closeDate, Guid clientId, Guid vehicleId, Guid statusId)
        {
            string error = string.Empty;

            if (id == Guid.Empty)
            {
                return (null, "Id cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                return (null, "Reason cannot be empty.");
            }

            if (reason.Length == 0)
            {
                return (null, "Reason must be at least 10 characters long.");
            }

            if (closeDate.HasValue && closeDate.Value < openDate)
            {
                return (null, "Close date cannot be earlier than open date.");
            }

            if (clientId == Guid.Empty)
            {
                return (null, "Client ID cannot be empty.");
            }

            if (vehicleId == Guid.Empty)
            {
                return (null, "Vehicle ID cannot be empty.");
            }

            if (statusId == Guid.Empty)
            {
                return (null, "Status ID cannot be empty.");
            }

            var item = new UserRequest(id, reason, openDate, closeDate, clientId, vehicleId, statusId);
            return (item, error);
        }
    }
}