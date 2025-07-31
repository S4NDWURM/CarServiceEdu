using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IWorkService
    {
        Task<Guid> CreateWork(Work model);
        Task<Guid> DeleteWork(Guid id);
        Task<List<Work>> GetAllWorks();
        Task<Work> GetWorkById(Guid id);
        Task<Guid> UpdateWork(Guid id, string name, string description, decimal cost);
    }
}