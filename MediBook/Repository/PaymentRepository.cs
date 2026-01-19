using MediBook.Data;
using MediBook.Interface;
using MediBook.Models;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly MediBookContext _context;

        public PaymentRepository(MediBookContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<Payment?> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.AppointmentId == appointmentId);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
