using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarService.Core.Models;
using CarService.DataAccess.Entities;

namespace CarService.DataAccess.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly CarServiceDbContext _ctx;
        public SpecializationRepository(CarServiceDbContext ctx) => _ctx = ctx;

        public async Task<List<Specialization>> Get() =>
            (await _ctx.Specializations.AsNoTracking().ToListAsync())
             .Select(e =>
             {
                 var (model, error) = Specialization.Create(e.Id, e.Name);
                 if (model == null)
                 {
                     throw new InvalidOperationException(error);
                 }
                 if (!string.IsNullOrEmpty(error))
                     throw new InvalidOperationException(error);
                 return model;
             })
             .ToList();

        public async Task<Specialization> GetById(Guid id)
        {
            var entity = await _ctx.Specializations
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
                throw new KeyNotFoundException($"Specialization with id {id} not found");

            var (model, error) = Specialization.Create(entity.Id, entity.Name);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }


        public async Task<Guid> Create(Specialization model)
        {
            var e = new SpecializationEntity
            {
                Id = model.Id,
                Name = model.Name
            };
            await _ctx.Specializations.AddAsync(e);
            await _ctx.SaveChangesAsync();
            return e.Id;
        }

        public async Task<Guid> Update(Guid id, string name)
        {
            var e = await _ctx.Specializations.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"Specialization with id {id} not found");
            e.Name = name;
            await _ctx.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var e = await _ctx.Specializations.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"Specialization with id {id} not found");
            _ctx.Specializations.Remove(e);
            await _ctx.SaveChangesAsync();
            return id;
        }
    }
}