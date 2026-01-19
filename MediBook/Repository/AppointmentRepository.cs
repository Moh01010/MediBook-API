using MediBook.Data;
using MediBook.Enum;
using MediBook.Interface;
using MediBook.Models;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly MediBookContext _context;

        public AppointmentRepository(MediBookContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(string userId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                .Include(a => a.Patient)
                .Include(a => a.Payment)
                .Where(a => a.Patient.UserId == userId)
                .OrderByDescending(a => a.Date)
                .ThenByDescending(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                .Include(a => a.Patient)
                .Include(a => a.Payment)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> IsSlotAvailableAsync(
            int doctorId,
            TimeSpan start,
            TimeSpan end,
            DateOnly date)
        {
            return !await _context.Appointments.AnyAsync(a =>
                a.DoctorId == doctorId &&
                a.Date == date &&
                a.StartTime < end &&
                a.EndTime > start &&
                a.Status != AppointmentStatus.Cancelled
            );
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
