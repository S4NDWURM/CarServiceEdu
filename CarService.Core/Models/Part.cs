namespace CarService.Core.Models
{
    public class Part
    {
        private Part(Guid id, string name, string article, decimal cost, Guid partBrandId)
        {
            Id = id;
            Name = name;
            Article = article;
            Cost = cost;
            PartBrandId = partBrandId;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Article { get; }
        public decimal Cost { get; }
        public Guid PartBrandId { get; }

        public static (Part? Item, string Error) Create(Guid id, string name, string article, decimal cost, Guid partBrandId)
        {
            if (id == Guid.Empty)
            {
                return (null, "Id cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return (null, "Name cannot be null or empty.");
            }

            if (name.Length > 100)
            {
                return (null, "Name cannot exceed 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(article))
            {
                return (null, "Article cannot be null or empty.");
            }

            if (article.Length > 50)
            {
                return (null, "Article cannot exceed 50 characters.");
            }

            if (cost <= 0)
            {
                return (null, "Cost must be greater than zero.");
            }

            if (partBrandId == Guid.Empty)
            {
                return (null, "Part brand ID cannot be empty.");
            }

            var item = new Part(id, name, article, cost, partBrandId);
            return (item, string.Empty);
        }
    }
}