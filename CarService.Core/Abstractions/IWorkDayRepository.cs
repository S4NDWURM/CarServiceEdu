using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IWorkDayRepository
    {
        Task<Guid> Create(WorkDay model);
        Task<Guid> Delete(Guid id);
        Task<List<WorkDay>> Get();
        Task<List<WorkDay>> GetByEmployeeId(Guid employeeId);
        Task<WorkDay> GetById(Guid id);
        Task<Guid> Update(Guid id, Guid employeeId, Guid typeOfDayId, TimeSpan startTime, TimeSpan endTime);
    }
}