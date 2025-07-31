using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarService.Core.Models;
using CarService.DataAccess.Entities;

namespace CarService.DataAccess.Repositories
{
    public class PlannedWorkRepository : IPlannedWorkRepository
    {
        private readonly CarServiceDbContext _ctx;
        public PlannedWorkRepository(CarServiceDbContext ctx) => _ctx = ctx;

        public async Task<List<PlannedWork>> Get() =>
            (await _ctx.PlannedWorks.AsNoTracking().ToListAsync())
             .Select(e =>
             {
                 var (model, error) = PlannedWork.Create(e.Id, e.PlanDate, e.ExpectedEndDate, e.TotalCost, e.WorkId, e.RequestId, e.StatusId);
                 if (!string.IsNullOrEmpty(error))
                     throw new InvalidOperationException(error);
                 if (model == null)
                     throw new InvalidOperationException("Failed to create PlannedWork model from entity.");
                 return model;
             })
             .ToList();
        public async Task<PlannedWork> GetById(Guid id)
        {
            var entity = await _ctx.PlannedWorks
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
                return null;

            var (model, error) = PlannedWork.Create(
                entity.Id, entity.PlanDate, entity.ExpectedEndDate, entity.TotalCost, entity.WorkId, entity.RequestId, entity.StatusId);

            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<PlannedWorkWithDetails> GetByIdDetailed(Guid id)
        {
            var entity = await _ctx.PlannedWorks
                                    .AsNoTracking()
                                    .Include(p => p.Work)       
                                    .Include(p => p.Status)     
                                    .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
                return null;

            
            var (model, error) = PlannedWorkWithDetails.Create(
                entity.Id,
                entity.PlanDate,
                entity.ExpectedEndDate,
                entity.TotalCost,
                entity.WorkId,
                entity.RequestId,
                entity.StatusId,
                entity.Work.Name,
                entity.Work.Description,
                entity.Work.Cost,
                entity.Status.Name
            );
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);
            return model;
        }


        public async Task<Guid> Create(PlannedWork model)
        {
            var e = new PlannedWorkEntity
            {
                Id = model.Id,
                PlanDate = model.PlanDate,
                ExpectedEndDate = model.ExpectedEndDate,
                TotalCost = model.TotalCost,
                WorkId = model.WorkId,
                RequestId = model.RequestId,
                StatusId = model.StatusId
            };
            await _ctx.PlannedWorks.AddAsync(e);
            await _ctx.SaveChangesAsync();
            return e.Id;
        }

        public async Task<Guid> Update(Guid id, DateTime planDate, DateTime expectedEndDate, decimal totalCost, Guid workId, Guid reqId, Guid statusId)
        {
            var e = await _ctx.PlannedWorks.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"PlannedWork with id {id} not found");
            e.PlanDate = planDate;
            e.ExpectedEndDate = expectedEndDate;
            e.TotalCost = totalCost;
            e.WorkId = workId;
            e.RequestId = reqId;
            e.StatusId = statusId;

            await _ctx.SaveChangesAsync();
            return id;
        }
        public async Task<List<PlannedWork>> GetByRequestId(Guid requestId)
        {
            var entities = await _ctx.PlannedWorks
                                      .AsNoTracking()
                                      .Where(p => p.RequestId == requestId)
                                      .ToListAsync();

            return entities.Select(e =>
            {
                var (model, error) = PlannedWork.Create(
                    e.Id, e.PlanDate, e.ExpectedEndDate, e.TotalCost, e.WorkId, e.RequestId, e.StatusId);
                if (model == null)
                {
                    throw new InvalidOperationException(error);
                }
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                return model;
            }).ToList();
        }

        public async Task<Guid> Delete(Guid id)
        {
            var e = await _ctx.PlannedWorks.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"PlannedWork with id {id} not found");
            _ctx.PlannedWorks.Remove(e);
            await _ctx.SaveChangesAsync();
            return id;
        }
    }
}