using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarService.Core.Models;
using CarService.DataAccess.Entities;

namespace CarService.DataAccess.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly CarServiceDbContext _ctx;
        public RequestRepository(CarServiceDbContext ctx) => _ctx = ctx;

        public async Task<List<UserRequest>> Get() =>
            (await _ctx.Requests.AsNoTracking().ToListAsync())
             .Select(e =>
             {
                 var (model, error) = UserRequest.Create(e.Id, e.Reason, e.OpenDate, e.CloseDate, e.ClientId, e.VehicleId, e.StatusId);
                 if (!string.IsNullOrEmpty(error))
                     throw new InvalidOperationException(error);
                 if (model == null)
                     throw new InvalidOperationException("Failed to create UserRequest model from entity.");
                 return model;
             })
             .ToList();
        public async Task<UserRequest> GetById(Guid id)
        {
            var entity = await _ctx.Requests
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(r => r.Id == id);

            if (entity == null)
                return null;

            var (model, error) = UserRequest.Create(
                entity.Id, entity.Reason, entity.OpenDate, entity.CloseDate, entity.ClientId, entity.VehicleId, entity.StatusId);

            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<UserRequestWithDetailsModel> GetByIdDetailed(Guid id)
        {
            var entity = await _ctx.Requests
                                   .AsNoTracking()
                                   .Include(r => r.Client)     
                                   .Include(r => r.Vehicle) 
                                   .Include(r => r.Status)    
                                   .FirstOrDefaultAsync(r => r.Id == id);

            if (entity == null)
                return null;

            var (model, error) = UserRequestWithDetailsModel.Create(
                   entity.Id,
                   entity.Reason,
                   entity.OpenDate,
                   entity.CloseDate,
                   new UserClientModel
                   {
                       Id = entity.Client.Id,
                       LastName = entity.Client.LastName,
                       FirstName = entity.Client.FirstName,
                       MiddleName = entity.Client.MiddleName
                   },
                   new UserVehicleModel
                   {
                       Id = entity.Vehicle.Id,
                       VIN = entity.Vehicle.VIN,
                       Year = entity.Vehicle.Year
                   },
                   new UserStatusModel
                   {
                       Id = entity.Status.Id,
                       Name = entity.Status.Name
                   }
               );

            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<List<UserRequest>> GetRequestsByClientId(Guid clientId) =>
            (await _ctx.Requests.AsNoTracking().Where(r => r.ClientId == clientId).ToListAsync())
             .Select(e =>
             {
                 var (model, error) = UserRequest.Create(e.Id, e.Reason, e.OpenDate, e.CloseDate, e.ClientId, e.VehicleId, e.StatusId);
                 if (!string.IsNullOrEmpty(error))
                     throw new InvalidOperationException(error);
                 if (model == null)
                     throw new InvalidOperationException("Failed to create UserRequest model from entity.");
                 return model;
             })
             .ToList();

        public async Task<Guid> Create(UserRequest model)
        {
            var e = new RequestEntity
            {
                Id = model.Id,
                Reason = model.Reason,
                OpenDate = model.OpenDate,
                CloseDate = model.CloseDate,
                ClientId = model.ClientId,
                VehicleId = model.VehicleId,
                StatusId = model.StatusId
            };
            await _ctx.Requests.AddAsync(e);
            await _ctx.SaveChangesAsync();
            return e.Id;
        }

        public async Task<Guid> Update(Guid id, string reason, DateTime openDate, DateTime? closeDate, Guid clientId, Guid vehicleId, Guid statusId)
        {
            var e = await _ctx.Requests.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"Request with id {id} not found");
            e.Reason = reason;
            e.OpenDate = openDate;
            e.CloseDate = closeDate;
            e.ClientId = clientId;
            e.VehicleId = vehicleId;
            e.StatusId = statusId;
            await _ctx.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> UpdateStatus(Guid id, Guid statusId)
        {
            var e = await _ctx.Requests.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"Request with id {id} not found");

            e.StatusId = statusId;
            await _ctx.SaveChangesAsync();
            return e.Id;
        }


        public async Task<Guid> Delete(Guid id)
        {
            var e = await _ctx.Requests.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"Request with id {id} not found");
            _ctx.Requests.Remove(e);
            await _ctx.SaveChangesAsync();
            return id;
        }
    }
}