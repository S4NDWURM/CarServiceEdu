using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IGenerationService
    {
        Task<Guid> CreateGeneration(Generation model);
        Task<Guid> DeleteGeneration(Guid id);
        Task<List<Generation>> GetAllGenerations();
        Task<Guid> UpdateGeneration(Guid id, Guid carModelId, string name, int startYear, int endYear);
        Task<Generation> GetGenerationById(Guid id);
        Task<List<Generation>> GetGenerationsByCarModelId(Guid carModelId);
    }
}