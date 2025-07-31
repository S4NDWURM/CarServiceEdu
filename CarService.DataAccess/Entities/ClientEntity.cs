namespace CarService.DataAccess.Entities
{
    public class ClientEntity
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public UserEntity User { get; set; }
        public ICollection<RequestEntity> Requests { get; set; } = [];
    }
}
