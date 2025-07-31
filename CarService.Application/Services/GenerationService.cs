using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class GenerationService : IGenerationService
    {
        private readonly IGenerationRepository _repo;
        public GenerationService(IGenerationRepository repo) => _repo = repo;

        public async Task<List<Generation>> GetAllGenerations() =>
            await _repo.Get();

        public async Task<Generation> GetGenerationById(Guid id)
        {
            return await _repo.GetById(id);
        }
        public async Task<List<Generation>> GetGenerationsByCarModelId(Guid carModelId) =>
            await _repo.GetByCarModelId(carModelId);


        public async Task<Guid> CreateGeneration(Generation model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateGeneration(Guid id, Guid carModelId, string name, int startYear, int endYear) =>
            await _repo.Update(id, carModelId, name, startYear, endYear);

        public async Task<Guid> DeleteGeneration(Guid id) =>
            await _repo.Delete(id);
    }
}
