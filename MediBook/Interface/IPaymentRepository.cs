using MediBook.Models;

namespace MediBook.Interface
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByAppointmentIdAsync(int appointmentId);
        Task CreateAsync(Payment payment);
        Task SaveAsync();
    }
}
