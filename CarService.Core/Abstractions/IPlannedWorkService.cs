using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IPlannedWorkService
    {
        Task<Guid> CreatePlannedWork(PlannedWork model);
        Task<Guid> DeletePlannedWork(Guid id);
        Task<List<PlannedWork>> GetAllPlannedWorks();
        Task<PlannedWork> GetPlannedWorkById(Guid id);
        Task<List<PlannedWork>> GetPlannedWorksByRequestId(Guid requestId);
        Task<PlannedWorkWithDetails> GetPlannedWorkWithDetailsById(Guid id);
        Task<Guid> UpdatePlannedWork(Guid id, DateTime planDate, DateTime expectedEndDate, decimal totalCost, Guid workId, Guid requestId, Guid statusId);
    }
}