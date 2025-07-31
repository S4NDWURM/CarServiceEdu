using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _repo;
        public StatusService(IStatusRepository repo) => _repo = repo;

        public async Task<List<Status>> GetAllStatuss() =>
            await _repo.Get();
        public async Task<Status> GetStatusById(Guid id)
        {
            return await _repo.GetById(id);
        }

        public async Task<Guid> CreateStatus(Status model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateStatus(Guid id, string name) =>
            await _repo.Update(id, name);

        public async Task<Guid> DeleteStatus(Guid id) =>
            await _repo.Delete(id);
    }
}
