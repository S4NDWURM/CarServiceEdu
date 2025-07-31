using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class PartBrandService : IPartBrandService
    {
        private readonly IPartBrandRepository _repo;
        public PartBrandService(IPartBrandRepository repo) => _repo = repo;

        public async Task<List<PartBrand>> GetAllPartBrands() =>
            await _repo.Get();


        public async Task<PartBrand> GetPartBrandById(Guid id)
        {
            return await _repo.GetById(id);
        }

        public async Task<Guid> CreatePartBrand(PartBrand model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdatePartBrand(Guid id, string name) =>
            await _repo.Update(id, name);

        public async Task<Guid> DeletePartBrand(Guid id) =>
            await _repo.Delete(id);
    }
}
