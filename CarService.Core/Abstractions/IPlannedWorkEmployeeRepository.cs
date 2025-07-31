using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IPlannedWorkEmployeeRepository
    {
        Task Create(PlannedWorkEmployee model);
        Task Delete(Guid plannedWorkId, Guid employeeId);
        Task<List<PlannedWorkEmployee>> Get();
        Task<List<PlannedWorkEmployee>> GetById(Guid plannedWorkId);
        Task<List<PlannedWorkEmployeeWithDetails>> GetByIdDetailed(Guid plannedWorkId);
    }
}