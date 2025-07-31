using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface ITypeOfDayRepository
    {
        Task<Guid> Create(TypeOfDay model);
        Task<Guid> Delete(Guid id);
        Task<List<TypeOfDay>> Get();
        Task<TypeOfDay> GetById(Guid id);
        Task<Guid> Update(Guid id, string name);
    }
}