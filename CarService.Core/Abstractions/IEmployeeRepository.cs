using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Guid> Create(Employee model);
        Task<Guid> Delete(Guid id);
        Task<List<Employee>> Get();
        Task<Employee> GetById(Guid id);
        Task<Guid> Update(Guid id, string lastName, string firstName, string middleName, int workExperience, DateTime hireDate, Guid employeeStatus);
    }
}