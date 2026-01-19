using MediBook.Data;
using MediBook.Interface;
using MediBook.Models;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Repository
{
    public class DoctorDayOffRepository : IDoctorDayOffRepository
    {
        private readonly MediBookContext _context;

        public DoctorDayOffRepository(MediBookContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DoctorDayOff dayOff)
        {
             _context.DoctorDaysOff.Add(dayOff);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DoctorDayOff dayOff)
        {
            _context.DoctorDaysOff.Remove(dayOff);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int doctorId, DateOnly date)
        {
            return await _context.DoctorDaysOff
                .AnyAsync(d => d.DoctorId == doctorId && d.Date == date);
        }

        public async Task<IEnumerable<DoctorDayOff>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.DoctorDaysOff
                .Where(d=>d.DoctorId == doctorId)
                .OrderBy(d=>d.Date).ToListAsync();
        }
    }
}
