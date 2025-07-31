using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarService.Core.Models;
using CarService.DataAccess.Entities;

namespace CarService.DataAccess.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly CarServiceDbContext _ctx;
        public StatusRepository(CarServiceDbContext ctx) => _ctx = ctx;

        public async Task<List<Status>> Get() =>
            (await _ctx.Statuses.AsNoTracking().ToListAsync())
             .Select(e =>
             {
                 var (model, error) = Status.Create(e.Id, e.Name);
                 if (model == null)
                 {
                     throw new InvalidOperationException(error);
                 }
                 if (!string.IsNullOrEmpty(error))
                     throw new InvalidOperationException(error);
                 return model;
             })
             .ToList();

        public async Task<Status> GetById(Guid id)
        {
            var entity = await _ctx.Statuses
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(s => s.Id == id);

            if (entity == null)
                return null;

            var (model, error) = Status.Create(entity.Id, entity.Name);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }


        public async Task<Guid> Create(Status model)
        {
            var e = new StatusEntity
            {
                Id = model.Id,
                Name = model.Name
            };
            await _ctx.Statuses.AddAsync(e);
            await _ctx.SaveChangesAsync();
            return e.Id;
        }

        public async Task<Guid> Update(Guid id, string name)
        {
            var e = await _ctx.Statuses.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"Status with id {id} not found");
            e.Name = name;
            await _ctx.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var e = await _ctx.Statuses.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"Status with id {id} not found");
            _ctx.Statuses.Remove(e);
            await _ctx.SaveChangesAsync();
            return id;
        }
    }
}