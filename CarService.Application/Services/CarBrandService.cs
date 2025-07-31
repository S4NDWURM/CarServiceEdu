using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class CarBrandService : ICarBrandService
    {
        private readonly ICarBrandRepository _repo;
        public CarBrandService(ICarBrandRepository repo) => _repo = repo;

        public async Task<List<CarBrand>> GetAllCarBrands() =>
            await _repo.Get();

        public async Task<CarBrand> GetCarBrandById(Guid id)
        {
            return await _repo.GetById(id);
        }


        public async Task<Guid> CreateCarBrand(CarBrand model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateCarBrand(Guid id, string name) =>
            await _repo.Update(id, name);

        public async Task<Guid> DeleteCarBrand(Guid id) =>
            await _repo.Delete(id);
    }
}
