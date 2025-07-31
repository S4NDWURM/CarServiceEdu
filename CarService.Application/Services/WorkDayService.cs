using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class WorkDayService : IWorkDayService
    {
        private readonly IWorkDayRepository _repo;
        public WorkDayService(IWorkDayRepository repo) => _repo = repo;

        public async Task<List<WorkDay>> GetAllWorkDays() =>
            await _repo.Get();

        public async Task<WorkDay> GetWorkDayById(Guid id) =>
            await _repo.GetById(id);

        public async Task<List<WorkDay>> GetWorkDaysByEmployeeId(Guid employeeId) =>
            await _repo.GetByEmployeeId(employeeId);


        public async Task<Guid> CreateWorkDay(WorkDay model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateWorkDay(Guid id, Guid employeeId, Guid typeOfDayId, TimeSpan startTime, TimeSpan endTime) =>
            await _repo.Update(id, employeeId, typeOfDayId, startTime, endTime);

        public async Task<Guid> DeleteWorkDay(Guid id) =>
            await _repo.Delete(id);
    }
}
