using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarService.Core.Models;
using CarService.DataAccess.Entities;

namespace CarService.DataAccess.Repositories
{
    public class WorkDayRepository : IWorkDayRepository
    {
        private readonly CarServiceDbContext _ctx;
        public WorkDayRepository(CarServiceDbContext ctx) => _ctx = ctx;

        public async Task<List<WorkDay>> Get() =>
            (await _ctx.WorkDays.AsNoTracking().ToListAsync())
             .Select(e =>
             {
                 var (model, error) = WorkDay.Create(e.Id, e.EmployeeId, e.TypeOfDayId, e.StartTime, e.EndTime);
                 if (model == null)
                 {
                     throw new InvalidOperationException(error);
                 }
                 if (!string.IsNullOrEmpty(error))
                     throw new InvalidOperationException(error);
                 return model;
             })
             .ToList();

        public async Task<WorkDay> GetById(Guid id)
        {
            var entity = await _ctx.WorkDays
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
                throw new KeyNotFoundException($"WorkDay with id {id} not found");

            var (model, error) = WorkDay.Create(entity.Id, entity.EmployeeId, entity.TypeOfDayId, entity.StartTime, entity.EndTime);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<List<WorkDay>> GetByEmployeeId(Guid employeeId)
        {
            var entities = await _ctx.WorkDays
                .AsNoTracking()
                .Where(e => e.EmployeeId == employeeId)
                .ToListAsync();

            var result = new List<WorkDay>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = WorkDay.Create(e.Id, e.EmployeeId, e.TypeOfDayId, e.StartTime, e.EndTime);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }


        public async Task<Guid> Create(WorkDay model)
        {
            var e = new WorkDayEntity
            {
                Id = model.Id,
                EmployeeId = model.EmployeeId,
                TypeOfDayId = model.TypeOfDayId,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };
            await _ctx.WorkDays.AddAsync(e);
            await _ctx.SaveChangesAsync();
            return e.Id;
        }

        public async Task<Guid> Update(Guid id, Guid employeeId, Guid typeOfDayId, TimeSpan startTime, TimeSpan endTime)
        {
            var e = await _ctx.WorkDays.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"WorkDay with id {id} not found");
            e.EmployeeId = employeeId;
            e.TypeOfDayId = typeOfDayId;
            e.StartTime = startTime;
            e.EndTime = endTime;
            await _ctx.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var e = await _ctx.WorkDays.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"WorkDay with id {id} not found");
            _ctx.WorkDays.Remove(e);
            await _ctx.SaveChangesAsync();
            return id;
        }
    }
}