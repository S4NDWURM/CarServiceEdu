using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class CarModelService : ICarModelService
    {
        private readonly ICarModelRepository _repo;
        public CarModelService(ICarModelRepository repo) => _repo = repo;

        public async Task<List<CarModel>> GetAllCarModels() =>
            await _repo.Get();

        public async Task<CarModel> GetCarModelById(Guid id)
        {
            return await _repo.GetById(id);
        }

        public async Task<List<CarModel>> GetCarModelsByCarBrandId(Guid carBrandId) =>
    await _repo.GetByCarBrandId(carBrandId);


        public async Task<Guid> CreateCarModel(CarModel model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateCarModel(Guid id, string name, Guid carBrandId) =>
            await _repo.Update(id, name, carBrandId);

        public async Task<Guid> DeleteCarModel(Guid id) =>
            await _repo.Delete(id);
    }
}
