using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class EmployeeSpecializationService : IEmployeeSpecializationService
    {
        private readonly IEmployeeSpecializationRepository _repo;
        public EmployeeSpecializationService(IEmployeeSpecializationRepository repo) => _repo = repo;

        public async Task<List<EmployeeSpecialization>> GetAllEmployeeSpecializations() =>
            await _repo.Get();

        public async Task<EmployeeSpecialization> GetEmployeeSpecializationById(Guid employeeId) =>
            await _repo.GetById(employeeId);

        public async Task<List<EmployeeSpecializationWithDetails>> GetEmployeeSpecializationByIdDetailed(Guid employeeId) =>
            await _repo.GetByIdDetailed(employeeId);


        public async Task CreateEmployeeSpecialization(EmployeeSpecialization model) =>
            await _repo.Create(model);

        public async Task DeleteEmployeeSpecialization(Guid employeeId, Guid specializationId) =>
            await _repo.Delete(employeeId, specializationId);
    }
}
