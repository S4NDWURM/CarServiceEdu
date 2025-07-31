using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IStatusService
    {
        Task<Guid> CreateStatus(Status model);
        Task<Guid> DeleteStatus(Guid id);
        Task<List<Status>> GetAllStatuss();
        Task<Status> GetStatusById(Guid id);
        Task<Guid> UpdateStatus(Guid id, string name);
    }
}