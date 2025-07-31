using CarService.Core.Models;
using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CarService.DataAccess.Repositories
{

    public class RoleRepository : IRoleRepository
    {
        private readonly CarServiceDbContext _context;

        public RoleRepository(CarServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Role> GetByName(string name)
        {
            var entity = await _context.Roles
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(r => r.Name == name);

            if (entity is null)
                return null;

            var (model, error) = Role.Create(entity.Id, entity.Name);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<Role> GetById(Guid id)
        {
            var entity = await _context.Roles
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(r => r.Id == id);

            if (entity is null)
                return null;

            var (model, error) = Role.Create(entity.Id, entity.Name);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<IReadOnlyCollection<Role>> GetAll()
        {
            var entities = await _context.Roles
                                         .AsNoTracking()
            .ToListAsync();

            var result = new List<Role>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = Role.Create(e.Id, e.Name);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);

                result.Add(model);
            }

            return result;
        }


        public async Task<Role> Create(Guid id, string name)
        {
            var existingRole = await _context.Roles
                                             .FirstOrDefaultAsync(r => r.Name == name);
            if (existingRole != null)
            {
                throw new InvalidOperationException($"Роль с именем '{name}' уже существует.");
            }

            var roleEntity = new RoleEntity
            {
                Id = id,
                Name = name
            };

            _context.Roles.Add(roleEntity);
            await _context.SaveChangesAsync();

            var (roleModel, error) = Role.Create(roleEntity.Id, roleEntity.Name);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return roleModel;
        }
    }
}
