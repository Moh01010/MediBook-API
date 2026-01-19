using MediBook.Data;
using MediBook.Interface;
using MediBook.Models;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Repository
{
    public class ClinicRepository : IClinicRepository
    {
        private readonly MediBookContext _context;

        public ClinicRepository(MediBookContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Clinic clinic)
        {
            _context.Clinics.Add(clinic);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Clinic>> GetAllAsync()
        {
            return await _context.Clinics.ToListAsync();
        }

        public async Task<Clinic?> GetByIdAsync(int id)
        {
            return await _context.Clinics.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
