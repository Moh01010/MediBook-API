using MediBook.Models;

namespace MediBook.Interface
{
    public interface IDoctorAvailableRepository
    {
        Task<IEnumerable<Appointment>> GetDoctorAppointmentsByDateAsync(int doctorId, DateOnly date);
    }
}
