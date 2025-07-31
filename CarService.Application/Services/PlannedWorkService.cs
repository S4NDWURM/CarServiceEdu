using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class PlannedWorkService : IPlannedWorkService
    {
        private readonly IPlannedWorkRepository _repo;
        public PlannedWorkService(IPlannedWorkRepository repo) => _repo = repo;

        public async Task<List<PlannedWork>> GetAllPlannedWorks() =>
            await _repo.Get();

        public async Task<PlannedWork> GetPlannedWorkById(Guid id)
        {
            return await _repo.GetById(id);
        }

        public async Task<PlannedWorkWithDetails> GetPlannedWorkWithDetailsById(Guid id)
        {
            return await _repo.GetByIdDetailed(id);
        }

        public async Task<List<PlannedWork>> GetPlannedWorksByRequestId(Guid requestId) =>
    await _repo.GetByRequestId(requestId);


        public async Task<Guid> CreatePlannedWork(PlannedWork model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdatePlannedWork(Guid id, DateTime planDate, DateTime expectedEndDate, decimal totalCost, Guid workId, Guid reqId, Guid statusId) =>
            await _repo.Update(id, planDate, expectedEndDate, totalCost, workId, reqId, statusId);

        public async Task<Guid> DeletePlannedWork(Guid id) =>
            await _repo.Delete(id);
    }
}
