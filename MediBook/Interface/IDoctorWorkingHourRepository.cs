using MediBook.Models;

namespace MediBook.Interface
{
    public interface IDoctorWorkingHourRepository
    {
        Task AddAsync(DoctorWorkingHour workingHour);
        Task<IEnumerable<DoctorWorkingHour>> GetByDoctorIdAsync(int doctorId);
        Task<IEnumerable<DoctorWorkingHour>> GetByDoctorAndDayAsync(int doctorId,DayOfWeek dayOfWeek);
    }
}
