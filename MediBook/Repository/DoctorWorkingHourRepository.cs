using MediBook.Data;
using MediBook.Interface;
using MediBook.Models;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Repository
{
    public class DoctorWorkingHourRepository : IDoctorWorkingHourRepository
    {
        private readonly MediBookContext _context;

        public DoctorWorkingHourRepository(MediBookContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DoctorWorkingHour workingHour)
        {
            _context.DoctorWorkingHours.Add(workingHour);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DoctorWorkingHour>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.DoctorWorkingHours
                .Where(w=>w.DoctorId == doctorId)
                .OrderBy(w => w.DayOfWeek)
                .ThenBy(w=>w.StartTime)
                .ToListAsync();
        }
        public async Task<IEnumerable<DoctorWorkingHour>> GetByDoctorAndDayAsync( int doctorId,DayOfWeek dayOfWeek)
        {
            return await _context.DoctorWorkingHours
                .Where(w =>
                    w.DoctorId == doctorId &&
                    w.DayOfWeek == dayOfWeek
                )
                .ToListAsync();
        }
    }
}
