using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class WorkService : IWorkService
    {
        private readonly IWorkRepository _repo;
        public WorkService(IWorkRepository repo) => _repo = repo;

        public async Task<List<Work>> GetAllWorks() =>
            await _repo.Get();

        public async Task<Work> GetWorkById(Guid id)
        {
            return await _repo.GetById(id);
        }


        public async Task<Guid> CreateWork(Work model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateWork(Guid id, string name, string description, decimal cost) =>
            await _repo.Update(id, name, description, cost);

        public async Task<Guid> DeleteWork(Guid id) =>
            await _repo.Delete(id);
    }
}
