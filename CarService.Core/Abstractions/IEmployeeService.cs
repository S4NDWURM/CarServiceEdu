using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IEmployeeService
    {
        Task<Guid> CreateEmployee(Employee model);
        Task<Guid> DeleteEmployee(Guid id);
        Task<List<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(Guid id);
        Task<Guid> UpdateEmployee(Guid id, string lastName, string firstName, string middleName, int workExperience, DateTime hireDate, Guid employeeStatus);
    }
}