namespace CarService.Core.Models
{
    public class PlannedWorkPart
    {
        private PlannedWorkPart(Guid plannedWorkId, Guid partId, int quantity)
        {
            PlannedWorkId = plannedWorkId;
            PartId = partId;
            Quantity = quantity;
        }

        public Guid PlannedWorkId { get; }
        public Guid PartId { get; }
        public int Quantity { get; }

        public static (PlannedWorkPart? Item, string Error) Create(Guid plannedWorkId, Guid partId, int quantity)
        {
            string error = string.Empty;

            if (plannedWorkId == Guid.Empty)
            {
                return (null, "Planned work ID cannot be empty.");
            }
            if (partId == Guid.Empty)
            {
                return (null, "Part ID cannot be empty.");
            }

            if (quantity <= 0)
            {
                return (null, "Quantity must be greater than zero.");
            }

            var item = new PlannedWorkPart(plannedWorkId, partId, quantity);
            return (item, string.Empty);
        }
    }
}
