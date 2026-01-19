using MediBook.Models;

namespace MediBook.Interface
{
    public interface IDoctorRepository
    {
        Task CreateAsync(Doctor doctor);
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor?> GetByIdAsync(int id);

    }
}
