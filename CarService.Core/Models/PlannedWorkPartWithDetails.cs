namespace CarService.Core.Models
{
    public class PlannedWorkPartWithDetails
    {
        public Guid PartId { get; }
        public string Name { get; }
        public string Article { get; }
        public decimal Cost { get; }
        public int Quantity { get; }
        public string BrandName { get; }

        private PlannedWorkPartWithDetails(Guid partId, string name, string article, decimal cost, int quantity, string brandName)
        {
            PartId = partId;
            Name = name;
            Article = article;
            Cost = cost;
            Quantity = quantity;
            BrandName = brandName;
        }


        public static (PlannedWorkPartWithDetails? Item, string Error) Create(Guid partId, string name, string article, decimal cost, int quantity, string brandName)
        {
            string error = string.Empty;

            if (partId == Guid.Empty)
            {
                return (null, "Part ID cannot be empty.");
            }

            if (string.IsNullOrEmpty(name))
            {
                return (null, "Part name cannot be empty.");
            }

            if (name.Length > 100)
            {
                return (null, "Part name cannot exceed 100 characters.");
            }

            if (string.IsNullOrEmpty(article))
            {
                return (null, "Part article cannot be empty.");
            }

            if (article.Length > 50)
            {
                return (null, "Part article cannot exceed 50 characters.");
            }

            if (cost <= 0)
            {
                return (null, "Cost must be greater than zero.");
            }

            if (quantity <= 0)
            {
                return (null, "Quantity must be greater than zero.");
            }

            if (string.IsNullOrEmpty(brandName))
            {
                return (null, "Brand name cannot be empty.");
            }

            if (brandName.Length > 100)
            {
                return (null, "Brand name cannot exceed 100 characters.");
            }

            var item = new PlannedWorkPartWithDetails(partId, name, article, cost, quantity, brandName);
            return (item, error);
        }
    }
}
