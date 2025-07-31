namespace CarService.Core.Models
{
    public class PlannedWork
    {
        private PlannedWork(Guid id, DateTime planDate, DateTime expectedEndDate, decimal totalCost, Guid workId, Guid requestId, Guid statusId)
        {
            Id = id;
            PlanDate = planDate;
            ExpectedEndDate = expectedEndDate;
            TotalCost = totalCost;
            WorkId = workId;
            RequestId = requestId;
            StatusId = statusId;
        }

        public Guid Id { get; }
        public DateTime PlanDate { get; }
        public DateTime ExpectedEndDate { get; }
        public int PartsQuantity { get; }
        public decimal TotalCost { get; }
        public Guid WorkId { get; }
        public Guid RequestId { get; }
        public Guid StatusId { get; }

        public static (PlannedWork? Item, string Error) Create(Guid id, DateTime planDate, DateTime expectedEndDate, decimal totalCost, Guid workId, Guid reqId, Guid statusId)
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

            if (workId == Guid.Empty)
            {
                return (null, "Work ID cannot be empty.");
            }

            if (reqId == Guid.Empty)
            {
                return (null, "Request ID cannot be empty.");
            }

            if (statusId == Guid.Empty)
            {
                return (null, "Status ID cannot be empty.");
            }

            var item = new PlannedWork(id, planDate, expectedEndDate, totalCost, workId, reqId, statusId);
            return (item, string.Empty);
        }
    }
}