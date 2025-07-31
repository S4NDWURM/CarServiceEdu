namespace CarService.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public RoleEntity Role { get; set; }

        public Guid? ClientId { get; set; }  
        public ClientEntity Client { get; set; }  

        public Guid? EmployeeId { get; set; } 
        public EmployeeEntity Employee { get; set; }  
    }
}
