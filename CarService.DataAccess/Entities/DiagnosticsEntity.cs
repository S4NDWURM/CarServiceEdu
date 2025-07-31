namespace CarService.DataAccess.Entities
{
    public class DiagnosticsEntity
    {
        public Guid Id { get; set; }
        public DateTime DiagnosticsDate { get; set; }
        public string ResultDescription { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid RequestId { get; set; }

        public EmployeeEntity Employee { get; set; }
        public RequestEntity Request { get; set; }
    }
}