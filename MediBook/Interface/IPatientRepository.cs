using MediBook.Models;

namespace MediBook.Interface
{
    public interface IPatientRepository
    {
        Task<Patient> GetByUserIdAsync(string userId);
        Task CreateAsync(Patient patient);
        Task DeleteAsync(Patient patient);
        Task SaveAsync();
    }
}
