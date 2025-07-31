using CarService.Core.Models;
using CarService.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarServiceDbContext _context;

        public UserRepository(CarServiceDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUserId(Guid userId)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (userEntity == null)
            {
                return null;
            }

            var (model, error) = User.Create(
                userEntity.Id,
                userEntity.UserName,
                userEntity.PasswordHash,
                userEntity.Email,
                userEntity.RoleId,
                userEntity.ClientId,
                userEntity.EmployeeId
            );

            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }


        public async Task Add(User user)
        {
            var userEntity = new UserEntity()
            {
                Id = user.Id,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                RoleId = user.RoleId,
                ClientId = user.ClientId, 
                EmployeeId = user.EmployeeId  
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (userEntity == null)
            {
                return null;
            }

            var (model, error) = User.Create(
                userEntity.Id,
                userEntity.UserName,
                userEntity.PasswordHash,
                userEntity.Email,
                userEntity.RoleId,
                userEntity.ClientId, 
                userEntity.EmployeeId  
            );

            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }
    }
}
