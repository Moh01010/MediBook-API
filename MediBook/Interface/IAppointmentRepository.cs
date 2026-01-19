using MediBook.Models;

namespace MediBook.Interface
{
    public interface IAppointmentRepository
    {
        Task<bool> IsSlotAvailableAsync(int doctorId, TimeSpan start, TimeSpan end, DateOnly date);
        Task CreateAsync(Appointment appointment);
        Task DeleteAsync(Appointment appointment);
        Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(string userId);
        Task <Appointment?> GetByIdAsync(int id);
        Task SaveAsync();
    }
}
