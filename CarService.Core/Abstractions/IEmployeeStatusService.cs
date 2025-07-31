
using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IEmployeeStatusService
    {
        Task<Guid> CreateEmployeeStatus(EmployeeStatus model);
        Task<Guid> DeleteEmployeeStatus(Guid id);
        Task<List<EmployeeStatus>> GetAllEmployeeStatuses();
        Task<EmployeeStatus> GetEmployeeStatusById(Guid id);
        Task<Guid> UpdateEmployeeStatus(Guid id, string name);
    }
}