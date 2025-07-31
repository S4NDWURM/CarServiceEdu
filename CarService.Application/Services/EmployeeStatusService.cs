using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class EmployeeStatusService : IEmployeeStatusService
    {
        private readonly IEmployeeStatusRepository _repo;
        public EmployeeStatusService(IEmployeeStatusRepository repo) => _repo = repo;

        public async Task<List<EmployeeStatus>> GetAllEmployeeStatuses() =>
            await _repo.Get();

        public async Task<EmployeeStatus> GetEmployeeStatusById(Guid id) =>
            await _repo.GetById(id);

        public async Task<Guid> CreateEmployeeStatus(EmployeeStatus model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateEmployeeStatus(Guid id, string name) =>
            await _repo.Update(id, name);

        public async Task<Guid> DeleteEmployeeStatus(Guid id) =>
            await _repo.Delete(id);
    }
}
