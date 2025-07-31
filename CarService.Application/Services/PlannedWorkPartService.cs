using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class PlannedWorkPartService : IPlannedWorkPartService
    {
        private readonly IPlannedWorkPartRepository _repo;
        public PlannedWorkPartService(IPlannedWorkPartRepository repo) => _repo = repo;

        public async Task<List<PlannedWorkPart>> GetAllPlannedWorkParts() =>
            await _repo.Get();

        public async Task<List<PlannedWorkPart>> GetPlannedWorkPartsById(Guid plannedWorkId)
        {
            return await _repo.GetById(plannedWorkId);
        }

        public async Task<List<PlannedWorkPartWithDetails>> GetPlannedWorkPartsWithDetails(Guid plannedWorkId)
        {
            return await _repo.GetByIdDetailed(plannedWorkId);
        }



        public async Task CreatePlannedWorkPart(PlannedWorkPart model) =>
            await _repo.Create(model);

        public async Task DeletePlannedWorkPart(Guid plannedWorkId, Guid partId) =>
            await _repo.Delete(plannedWorkId, partId);
    }
}
