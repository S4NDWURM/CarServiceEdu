using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IEmployeeStatusRepository
    {
        Task<Guid> Create(EmployeeStatus model);
        Task<Guid> Delete(Guid id);
        Task<List<EmployeeStatus>> Get();
        Task<EmployeeStatus> GetById(Guid id);
        Task<Guid> Update(Guid id, string name);
    }
}