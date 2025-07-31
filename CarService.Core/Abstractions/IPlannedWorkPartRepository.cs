using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IPlannedWorkPartRepository
    {
        Task Create(PlannedWorkPart model);
        Task Delete(Guid plannedWorkId, Guid partId);
        Task<List<PlannedWorkPart>> Get();
        Task<List<PlannedWorkPart>> GetById(Guid plannedWorkId);
        Task<List<PlannedWorkPartWithDetails>> GetByIdDetailed(Guid plannedWorkId);
    }
}