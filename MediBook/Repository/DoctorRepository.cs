using MediBook.Data;
using MediBook.Interface;
using MediBook.Models;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly MediBookContext _context;

        public DoctorRepository(MediBookContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .Include(d=>d.User)
                .Include(d=>d.Clinic)
                .ToListAsync();
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Clinic)
                .FirstOrDefaultAsync(d=>d.Id == id);
        }
    }
}
