using MediBook.Enum;

namespace MediBook.Dtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateOnly Date {  get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
