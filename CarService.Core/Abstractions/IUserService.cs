
using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IUserService
    {
        Task<Guid> GetClientIdByUserId(Guid userId);
        Task<Guid> GetEmployeeIdByUserId(Guid userId);
        Task<string> Login(string email, string password);
        Task RegisterClient(string userName, string email, string password, Guid roleId, string lastName, string firstName, string middleName, DateTime dateOfBirth, DateTime registrationDate);
        Task RegisterEmployee(string userName, string email, string password, Guid roleId, Guid employeeStatusId, string lastName, string firstName, string middleName, int workExperience, DateTime hireDate);
    }
}