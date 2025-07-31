using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IGenerationRepository
    {
        Task<Guid> Create(Generation model);
        Task<Guid> Delete(Guid id);
        Task<List<Generation>> Get();
        Task<Guid> Update(Guid id, Guid carModelId, string name, int startYear, int endYear);
        Task<Generation> GetById(Guid id);
        Task<List<Generation>> GetByCarModelId(Guid carModelId);
    }
}