using MediBook.Data;
using MediBook.Interface;
using MediBook.Models;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly MediBookContext _context;

        public PatientRepository(MediBookContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Patient patient)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<Patient> GetByUserIdAsync(string userId)
        {
            return await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
