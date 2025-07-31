namespace CarService.Core.Models
{
    public class Generation
    {
        private Generation(Guid id, Guid carModelId, string name, int startYear, int endYear)
        {
            Id = id;
            CarModelId = carModelId;
            Name = name;
            StartYear = startYear;
            EndYear = endYear;
        }

        public Guid Id { get; }
        public Guid CarModelId { get; }
        public string Name { get; }
        public int StartYear { get; }
        public int EndYear { get; }

        public static (Generation? Item, string Error) Create(
            Guid id, Guid carModelId, string name, int start, int end)
        {
            if (carModelId == Guid.Empty)
                return (null, "CarModelId is required.");

            if (string.IsNullOrWhiteSpace(name))
                return (null, "Name is required.");

            name = name.Trim();

            if (name.Length > 100)
                return (null, "Name cannot exceed 100 characters.");

            if (start <= 1800)
                return (null, "Start year must be greater than 1800.");

            if (end < start)
                return (null, "End year cannot be earlier than start year.");


            return (new Generation(id, carModelId, name, start, end), string.Empty);
        }
    }
}
