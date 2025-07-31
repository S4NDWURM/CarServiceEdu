using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly ISpecializationRepository _repo;
        public SpecializationService(ISpecializationRepository repo) => _repo = repo;

        public async Task<List<Specialization>> GetAllSpecializations() =>
            await _repo.Get();

        public async Task<Specialization> GetSpecializationById(Guid id) =>
            await _repo.GetById(id);


        public async Task<Guid> CreateSpecialization(Specialization model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateSpecialization(Guid id, string name) =>
            await _repo.Update(id, name);

        public async Task<Guid> DeleteSpecialization(Guid id) =>
            await _repo.Delete(id);
    }
}
