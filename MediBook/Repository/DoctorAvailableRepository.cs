using MediBook.Data;
using MediBook.Enum;
using MediBook.Interface;
using MediBook.Models;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Repository
{
    public class DoctorAvailableRepository : IDoctorAvailableRepository
    {
        private readonly MediBookContext _context;

        public DoctorAvailableRepository(MediBookContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetDoctorAppointmentsByDateAsync(int doctorId, DateOnly date)
        {
            return await _context.Appointments
                .Where(a=>
                a.DoctorId == doctorId &&
                a.Date == date&&
                a.Status!= AppointmentStatus.Completed
                ).ToListAsync();
        }
    }
}
