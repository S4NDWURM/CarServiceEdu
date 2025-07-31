using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<IReadOnlyCollection<Role>> GetAll()
            => _roleRepository.GetAll();

        public Task<Role> GetByName(string name)
            => _roleRepository.GetByName(name);
    }
}
