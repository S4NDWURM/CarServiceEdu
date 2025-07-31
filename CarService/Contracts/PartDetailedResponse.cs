namespace CarService.API.Contracts
{
    public class PartDetailedResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Article { get; set; }
        public decimal Cost { get; set; }
        public Guid PartBrandId { get; set; }

        public PartDetailedResponse(Guid id, string name, string article, decimal cost, Guid partBrandId)
        {
            Id = id;
            Name = name;
            Article = article;
            Cost = cost;
            PartBrandId = partBrandId;
        }
    }
}
