using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IPlannedWorkRepository
    {
        Task<Guid> Create(PlannedWork model);
        Task<Guid> Delete(Guid id);
        Task<List<PlannedWork>> Get();
        Task<PlannedWork> GetById(Guid id);
        Task<PlannedWorkWithDetails> GetByIdDetailed(Guid id);
        Task<List<PlannedWork>> GetByRequestId(Guid requestId);
        Task<Guid> Update(Guid id, DateTime planDate, DateTime expectedEndDate, decimal totalCost, Guid workId, Guid requestId, Guid statusId);
    }
}