namespace CarService.Core.Models
{
    public class PlannedWorkWithDetails
    {
        public Guid Id { get; }
        public DateTime PlanDate { get; }
        public DateTime ExpectedEndDate { get; }
        public decimal TotalCost { get; }
        public Guid WorkId { get; }
        public Guid RequestId { get; }
        public Guid StatusId { get; }
        public string WorkName { get; }
        public string WorkDesctiption { get; }
        public decimal WorkCost { get; }
        public string StatusName { get; }

        private PlannedWorkWithDetails(Guid id, DateTime planDate, DateTime expectedEndDate, decimal totalCost,
                                      Guid workId, Guid requestId, Guid statusId, string workName, string description, decimal cost, string statusName)
        {
            Id = id;
            PlanDate = planDate;
            ExpectedEndDate = expectedEndDate;
            TotalCost = totalCost;
            WorkId = workId;
            RequestId = requestId;
            StatusId = statusId;
            WorkName = workName;
            WorkDesctiption = description;
            WorkCost = cost;
            StatusName = statusName;
        }

        public static (PlannedWorkWithDetails? Item, string Error) Create(Guid id, DateTime planDate, DateTime expectedEndDate, decimal totalCost,
                                      Guid workId, Guid requestId, Guid statusId, string workName, string description, decimal cost, string statusName)
        {
            string error = string.Empty;

            if (id == Guid.Empty)
            {
                return (null, "Id cannot be empty.");
            }

            if (expectedEndDate < planDate)
            {
                return (null, "Expected end date cannot be earlier than plan date.");
            }

            if (totalCost <= 0)
            {
                return (null, "Total cost must be greater than zero.");
            }

            if (workId == Guid.Empty || requestId == Guid.Empty || statusId == Guid.Empty)
            {
                return (null, "Work ID, Request ID, and Status ID cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(workName) || workName.Length > 100)
            {
                return (null, "Work name cannot be empty and cannot exceed 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(description) || description.Length > 500)
            {
                return (null, "Work description cannot be empty and cannot exceed 500 characters.");
            }

            if (cost <= 0)
            {
                return (null, "Work cost must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(statusName) || statusName.Length > 100)
            {
                return (null, "Status name cannot be empty and cannot exceed 100 characters.");
            }


            var item = new PlannedWorkWithDetails(id, planDate, expectedEndDate, totalCost, workId, requestId, statusId, workName, description, cost, statusName);
            return (item, error);
        }
    }
}
