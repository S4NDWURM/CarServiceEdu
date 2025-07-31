using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class TypeOfDayService : ITypeOfDayService
    {
        private readonly ITypeOfDayRepository _repo;
        public TypeOfDayService(ITypeOfDayRepository repo) => _repo = repo;

        public async Task<List<TypeOfDay>> GetAllTypeOfDays() =>
            await _repo.Get();

        public async Task<TypeOfDay> GetTypeOfDayById(Guid id) =>
            await _repo.GetById(id);

        public async Task<Guid> CreateTypeOfDay(TypeOfDay model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateTypeOfDay(Guid id, string name) =>
            await _repo.Update(id, name);

        public async Task<Guid> DeleteTypeOfDay(Guid id) =>
            await _repo.Delete(id);
    }
}
