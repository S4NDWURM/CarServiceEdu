namespace CarService.API.Contracts
{
    public class PlannedWorkPartWithDetailsResponse
    {
        public Guid PartId { get; }
        public string Name { get; }
        public string Article { get; }
        public decimal Cost { get; }
        public int Quantity { get; }
        public string BrandName { get; }

        public PlannedWorkPartWithDetailsResponse(Guid partId, string name, string article, decimal cost, int quantity, string brandName)
        {
            PartId = partId;
            Name = name;
            Article = article;
            Cost = cost;
            Quantity = quantity;
            BrandName = brandName;
        }
    }

}
