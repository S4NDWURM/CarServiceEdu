using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;
        public EmployeeService(IEmployeeRepository repo) => _repo = repo;

        public async Task<List<Employee>> GetAllEmployees() =>
            await _repo.Get();

        public async Task<Employee> GetEmployeeById(Guid id) =>
            await _repo.GetById(id);

        public async Task<Guid> CreateEmployee(Employee model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateEmployee(Guid id, string lastName, string firstName, string middleName, int workExperience, DateTime hireDate, Guid employeeStatus) =>
            await _repo.Update(id, lastName, firstName, middleName, workExperience, hireDate, employeeStatus);

        public async Task<Guid> DeleteEmployee(Guid id) =>
            await _repo.Delete(id);
    }
}
