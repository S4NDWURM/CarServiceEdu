using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarService.Core.Models;
using CarService.DataAccess.Entities;

namespace CarService.DataAccess.Repositories
{
    public class TypeOfDayRepository : ITypeOfDayRepository
    {
        private readonly CarServiceDbContext _ctx;
        public TypeOfDayRepository(CarServiceDbContext ctx) => _ctx = ctx;

        public async Task<List<TypeOfDay>> Get() =>
            (await _ctx.TypesOfDay.AsNoTracking().ToListAsync())
             .Select(e =>
             {
                 var (model, error) = TypeOfDay.Create(e.Id, e.Name);
                 if (model == null)
                 {
                     throw new InvalidOperationException(error);
                 }
                 if (!string.IsNullOrEmpty(error))
                     throw new InvalidOperationException(error);
                 return model;
             })
             .ToList();

        public async Task<TypeOfDay> GetById(Guid id)
        {
            var entity = await _ctx.TypesOfDay
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
                throw new KeyNotFoundException($"TypeOfDay with id {id} not found");

            var (model, error) = TypeOfDay.Create(entity.Id, entity.Name);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }


        public async Task<Guid> Create(TypeOfDay model)
        {
            var e = new TypeOfDayEntity
            {
                Id = model.Id,
                Name = model.Name
            };
            await _ctx.TypesOfDay.AddAsync(e);
            await _ctx.SaveChangesAsync();
            return e.Id;
        }

        public async Task<Guid> Update(Guid id, string name)
        {
            var e = await _ctx.TypesOfDay.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"TypeOfDay with id {id} not found");
            e.Name = name;
            await _ctx.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var e = await _ctx.TypesOfDay.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"TypeOfDay with id {id} not found");
            _ctx.TypesOfDay.Remove(e);
            await _ctx.SaveChangesAsync();
            return id;
        }
    }
}