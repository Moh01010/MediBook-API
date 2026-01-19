using MediBook.Models;

namespace MediBook.Interface
{
    public interface IDoctorDayOffRepository
    {
        Task AddAsync(DoctorDayOff dayOff);
        Task<IEnumerable<DoctorDayOff>> GetByDoctorIdAsync(int doctorId);
        Task<bool> ExistsAsync(int doctorId, DateOnly date);
        Task DeleteAsync(DoctorDayOff dayOff);
    }
}
