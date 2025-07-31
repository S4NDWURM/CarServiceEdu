namespace CarService.Core.Models
{
    public class Diagnostics
    {
        private Diagnostics(Guid id, DateTime diagnosticsDate, string resultDescription, Guid employeeId, Guid requestId)
        {
            Id = id;
            DiagnosticsDate = diagnosticsDate;
            ResultDescription = resultDescription;
            EmployeeId = employeeId;
            RequestId = requestId;
        }

        public Guid Id { get; }
        public DateTime DiagnosticsDate { get; }
        public string ResultDescription { get; }
        public Guid EmployeeId { get; }
        public Guid RequestId { get; }

        public static (Diagnostics? Item, string Error) Create(Guid id, DateTime diagnosticsDate, string resultDescription, Guid employeeId, Guid requestId)
        {
            if (id == Guid.Empty)
            {
                return (null, "Id cannot be empty.");
            }

            if (string.IsNullOrEmpty(resultDescription))
            {
                return (null, "Result description cannot be null or empty.");
            }

            if (resultDescription.Length > 1000)
            {
                return (null, "Result description cannot exceed 1000 characters.");
            }

            if (employeeId == Guid.Empty)
            {
                return (null, "Employee ID cannot be empty.");
            }

            if (requestId == Guid.Empty)
            {
                return (null, "Request ID cannot be empty.");
            }

            var item = new Diagnostics(id, diagnosticsDate, resultDescription, employeeId, requestId);
            return (item, string.Empty);
        }
    }
}
