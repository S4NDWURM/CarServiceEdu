using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IWorkDayService
    {
        Task<Guid> CreateWorkDay(WorkDay model);
        Task<Guid> DeleteWorkDay(Guid id);
        Task<List<WorkDay>> GetAllWorkDays();
        Task<WorkDay> GetWorkDayById(Guid id);
        Task<List<WorkDay>> GetWorkDaysByEmployeeId(Guid employeeId);
        Task<Guid> UpdateWorkDay(Guid id, Guid employeeId, Guid typeOfDayId, TimeSpan startTime, TimeSpan endTime);
    }
}