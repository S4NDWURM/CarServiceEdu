using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarService.Core.Models;
using CarService.DataAccess.Entities;

namespace CarService.DataAccess.Repositories
{
    public class WorkRepository : IWorkRepository
    {
        private readonly CarServiceDbContext _ctx;
        public WorkRepository(CarServiceDbContext ctx) => _ctx = ctx;

        public async Task<List<Work>> Get() =>
            (await _ctx.Works.AsNoTracking().ToListAsync())
             .Select(e =>
             {
                 var (model, error) = Work.Create(e.Id, e.Name, e.Description, e.Cost);
                 if (model == null)
                 {
                     throw new InvalidOperationException(error);
                 }
                 if (!string.IsNullOrEmpty(error))
                     throw new InvalidOperationException(error);
                 return model;
             })
             .ToList();
        public async Task<Work> GetById(Guid id)
        {
            var entity = await _ctx.Works
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(w => w.Id == id);

            if (entity == null)
                return null;

            var (model, error) = Work.Create(entity.Id, entity.Name, entity.Description, entity.Cost);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }


        public async Task<Guid> Create(Work model)
        {
            var e = new WorkEntity
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Cost = model.Cost
            };
            await _ctx.Works.AddAsync(e);
            await _ctx.SaveChangesAsync();
            return e.Id;
        }

        public async Task<Guid> Update(Guid id, string name, string description, decimal cost)
        {
            var e = await _ctx.Works.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"Work with id {id} not found");
            e.Name = name;
            e.Description = description;
            e.Cost = cost;
            await _ctx.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var e = await _ctx.Works.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"Work with id {id} not found");
            _ctx.Works.Remove(e);
            await _ctx.SaveChangesAsync();
            return id;
        }
    }
}