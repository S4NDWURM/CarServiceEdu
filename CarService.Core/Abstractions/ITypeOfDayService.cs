using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface ITypeOfDayService
    {
        Task<Guid> CreateTypeOfDay(TypeOfDay model);
        Task<Guid> DeleteTypeOfDay(Guid id);
        Task<List<TypeOfDay>> GetAllTypeOfDays();
        Task<TypeOfDay> GetTypeOfDayById(Guid id);
        Task<Guid> UpdateTypeOfDay(Guid id, string name);
    }
}