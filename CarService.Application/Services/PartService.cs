using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class PartService :IPartService
    {
        private readonly IPartRepository _repo;
        public PartService(IPartRepository repo) => _repo = repo;

        public async Task<List<Part>> GetAllParts() =>
            await _repo.Get();
        public async Task<Part> GetPartById(Guid id)
        {
            return await _repo.GetById(id);
        }

        public async Task<List<Part>> SearchPartsByName(string name) =>
            await _repo.SearchByName(name);


        public async Task<Guid> CreatePart(Part model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdatePart(Guid id, string name, string article, decimal cost, Guid partBrandId) =>
            await _repo.Update(id, name, article, cost, partBrandId);

        public async Task<Guid> DeletePart(Guid id) =>
            await _repo.Delete(id);
    }
}
