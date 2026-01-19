using MediBook.Models;

namespace MediBook.Interface
{
    public interface IClinicRepository
    {
        Task CreateAsync(Clinic clinic);
        Task<IEnumerable<Clinic>> GetAllAsync();
        Task<Clinic?> GetByIdAsync(int id);
        Task SaveAsync();
    }
}
