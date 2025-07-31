using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class PlannedWorkEmployeeService : IPlannedWorkEmployeeService
    {
        private readonly IPlannedWorkEmployeeRepository _repo;
        public PlannedWorkEmployeeService(IPlannedWorkEmployeeRepository repo) => _repo = repo;

        public async Task<List<PlannedWorkEmployee>> GetAllPlannedWorkEmployees() =>
            await _repo.Get();

        public async Task<List<PlannedWorkEmployee>> GetPlannedWorkEmployeesById(Guid plannedWorkId)
        {
            return await _repo.GetById(plannedWorkId);
        }

        public async Task<List<PlannedWorkEmployeeWithDetails>> GetPlannedWorkEmployeesWithDetailsById(Guid plannedWorkId)
        {
            return await _repo.GetByIdDetailed(plannedWorkId);
        }



        public async Task CreatePlannedWorkEmployee(PlannedWorkEmployee model) =>
            await _repo.Create(model);

        public async Task DeletePlannedWorkEmployee(Guid plannedWorkId, Guid employeeId) =>
            await _repo.Delete(plannedWorkId, employeeId);
    }
}
