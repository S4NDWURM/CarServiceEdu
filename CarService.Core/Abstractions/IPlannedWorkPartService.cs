using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IPlannedWorkPartService
    {
        Task CreatePlannedWorkPart(PlannedWorkPart model);
        Task DeletePlannedWorkPart(Guid plannedWorkId, Guid partId);
        Task<List<PlannedWorkPart>> GetAllPlannedWorkParts();
        Task<List<PlannedWorkPart>> GetPlannedWorkPartsById(Guid plannedWorkId);
        Task<List<PlannedWorkPartWithDetails>> GetPlannedWorkPartsWithDetails(Guid plannedWorkId);
    }
}