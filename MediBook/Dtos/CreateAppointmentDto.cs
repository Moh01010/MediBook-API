namespace MediBook.Dtos
{
    public class CreateAppointmentDto
    {
        public int DoctorId { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateOnly Date { get; set; }
    }
}
