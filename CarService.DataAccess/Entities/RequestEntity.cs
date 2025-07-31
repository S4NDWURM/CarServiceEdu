namespace CarService.DataAccess.Entities
{
    public class RequestEntity
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public Guid ClientId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid StatusId { get; set; }

        public ClientEntity Client { get; set; }
        public VehicleEntity Vehicle { get; set; }
        public StatusEntity Status { get; set; }
        public ICollection<DiagnosticsEntity> Diagnostics { get; set; } = [];
        public ICollection<PlannedWorkEntity> PlannedWorks { get; set; } = [];
    }
}